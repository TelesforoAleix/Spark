using Xunit;
using Microsoft.AspNetCore.Mvc;
using backendSpark.API.Controllers;
using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace backendSpark.Tests.Controllers
{
    public class AttendeeControllerTests
    {
        [Fact]
        public void GetAttendee_ReturnsOk_WhenAttendeeExists()
        {
            // Arrange - Create test data
            var testAttendee = new Attendee
            {
                attendeeId = "TEST01",
                eventId = 1,
                firstName = "Test",
                lastName = "User",
                email = "test@example.com",
                header = "Test Position",
                bio = "Test Bio",
                link = "https://example.com"
            };
            
            // Create test repository and controller
            var repository = new TestRepository(testAttendee);
            var controller = new AttendeeController(repository);
            
            // Act - Call the controller method
            var result = controller.GetAttendee("TEST01");
            
            // Assert - Verify the result
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAttendee = Assert.IsType<Attendee>(okResult.Value);
            Assert.Equal("TEST01", returnedAttendee.attendeeId);
            Assert.Equal("Test", returnedAttendee.firstName);
        }
        
        [Fact]
        public void GetAttendee_ReturnsNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var repository = new TestRepository(null);
            var controller = new AttendeeController(repository);
            
            // Act
            var result = controller.GetAttendee("NONEXISTENT");
            
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        // Custom test repository implementation
        private class TestRepository : AttendeeRepository
        {
            private readonly Attendee _testAttendee;
            
            public TestRepository(Attendee testAttendee) : base(GetTestConfiguration())
            {
                _testAttendee = testAttendee;
            }
            
            // Helper method to create a minimal configuration for the base class
            private static IConfiguration GetTestConfiguration()
            {
                var inMemorySettings = new Dictionary<string, string> {
                    {"ConnectionStrings:spark_db", "test_connection_string"}
                };
                
                return new ConfigurationBuilder()
                    .AddInMemoryCollection(inMemorySettings)
                    .Build();
            }
            
            // Override repository methods for testing
            public override Attendee GetAttendeeById(string attendeeId)
            {
                if (_testAttendee == null) return null;
                return _testAttendee.attendeeId == attendeeId ? _testAttendee : null;
            }
            
            public override List<Attendee> GetAttendees()
            {
                return _testAttendee == null ? new List<Attendee>() : new List<Attendee> { _testAttendee };
            }
            
            // Stub other repository methods
            public override bool InsertAttendee(Attendee a) => true;
            public override bool UpdateAttendee(Attendee a) => true;
            public override bool DeleteAttendee(string attendeeId) => true;
        }
    }
}