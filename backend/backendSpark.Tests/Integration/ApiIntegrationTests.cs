using Xunit;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using backendSpark.Model.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace backendSpark.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Create a client using the factory
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    // Configure test services if needed
                    builder.ConfigureServices(services =>
                    {
                        // You can replace services here for testing
                    });
                })
                .CreateClient();

            // Add authentication if needed
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("admin:admin123"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }

        #region Authentication Tests

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new { username = "admin", password = "admin123" };
            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);

            Assert.True(result.TryGetProperty("headerValue", out _));
            Assert.True(result.TryGetProperty("userId", out _));
            Assert.True(result.TryGetProperty("username", out _));
        }

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginRequest = new { username = "invalid", password = "invalid" };
            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task Api_ReturnsUnauthorized_WhenNoAuthenticationProvided()
        {
            // Arrange
            var clientWithoutAuth = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5193")
            }; 

            // Act
            var response = await clientWithoutAuth.GetAsync("/api/Attendee");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region Attendee Tests

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task GetAttendees_ReturnsOkWithAttendees()
        {
            // Act
            var response = await _client.GetAsync("/api/Attendee");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var attendees = JsonSerializer.Deserialize<List<Attendee>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(attendees);
            Assert.True(attendees.Count > 0);
        }

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task GetAttendee_ReturnsAttendee_WhenAttendeeExists()
        {
            // This test requires an existing attendee in the database
            // Replace "AT0001" with a known attendee ID in your database
            var attendeeId = "AT0001";

            // Act
            var response = await _client.GetAsync($"/api/Attendee/{attendeeId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var attendee = JsonSerializer.Deserialize<Attendee>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(attendee);
            Assert.Equal(attendeeId, attendee.attendeeId);
        }

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task GetAttendee_ReturnsNotFound_WhenAttendeeDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/Attendee/NONEXISTENT");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task AttendeeEndToEndTest()
        {
            // This test performs a complete CRUD cycle for an attendee

            // Generate a unique ID to avoid conflicts with existing data
            var uniqueId = $"TE{Guid.NewGuid().ToString().Substring(0, 6)}";

            // 1. Create test attendee
            var newAttendee = new Attendee
            {
                attendeeId = uniqueId,
                eventId = 1,
                firstName = "Integration",
                lastName = "Test",
                email = "integration@test.com",
                hashed_password = "TestPassword123",
                header = "Test Profile",
                bio = "Created for integration testing",
                link = "https://example.com"
            };

            var createContent = new StringContent(
                JsonSerializer.Serialize(newAttendee),
                Encoding.UTF8,
                "application/json");

            // 2. Create the attendee
            var createResponse = await _client.PostAsync("/api/Attendee", createContent);

            // Log error response for debugging
            if (createResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorContent = await createResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Server Error: {errorContent}");
            }
            // 3. Get the created attendee
            var getResponse = await _client.GetAsync($"/api/Attendee/{uniqueId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var content = await getResponse.Content.ReadAsStringAsync();
            var retrievedAttendee = JsonSerializer.Deserialize<Attendee>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(uniqueId, retrievedAttendee.attendeeId);
            Assert.Equal("Integration", retrievedAttendee.firstName);

            // 4. Update the attendee
            retrievedAttendee.firstName = "Updated";
            retrievedAttendee.bio = "Updated for integration testing";

            var updateContent = new StringContent(
                JsonSerializer.Serialize(retrievedAttendee),
                Encoding.UTF8,
                "application/json");

            var updateResponse = await _client.PutAsync($"/api/Attendee/{uniqueId}", updateContent);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            // 5. Get the updated attendee
            var getUpdatedResponse = await _client.GetAsync($"/api/Attendee/{uniqueId}");
            var updatedContent = await getUpdatedResponse.Content.ReadAsStringAsync();
            var updatedAttendee = JsonSerializer.Deserialize<Attendee>(
                updatedContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("Updated", updatedAttendee.firstName);
            Assert.Equal("Updated for integration testing", updatedAttendee.bio);

            // 6. Delete the attendee
            var deleteResponse = await _client.DeleteAsync($"/api/Attendee/{uniqueId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // 7. Verify deletion - should return NotFound
            var getDeletedResponse = await _client.GetAsync($"/api/Attendee/{uniqueId}");
            Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
        }

        #endregion

        #region Event Tests

        [Fact]
        // [Fact(Skip = "Integration test - requires running API")]
        public async Task GetEvent_ReturnsEvent_WhenEventExists()
        {
            // Act
            var response = await _client.GetAsync("/api/Event");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var eventData = JsonSerializer.Deserialize<Event>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(eventData);
            Assert.True(eventData.EventId > 0);
            Assert.False(string.IsNullOrEmpty(eventData.Name));
        }

        #endregion

        #region Helper Methods

        // Helper method to generate a random attendee
        private Attendee CreateRandomAttendee()
        {
            var randomId = $"TA{Guid.NewGuid().ToString().Substring(0, 8)}";

            return new Attendee
            {
                attendeeId = randomId,
                eventId = 1,
                firstName = $"Test{randomId.Substring(0, 4)}",
                lastName = "User",
                email = $"test{randomId.Substring(0, 4)}@example.com",
                header = "Test Position",
                bio = "This is a test attendee created for integration testing.",
                link = "https://example.com"
            };
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        #endregion
    }
}