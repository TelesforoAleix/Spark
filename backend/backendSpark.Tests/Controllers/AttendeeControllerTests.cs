using Moq;
using Microsoft.AspNetCore.Mvc;
using backendSpark.API.Controllers;
using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;

namespace backendSpark.Tests.Controllers
{
    public class AttendeeControllerTests
    {
        private readonly Mock<AttendeeRepository> _mockRepository;
        private readonly AttendeeController _controller;

        public AttendeeControllerTests()
        {
            // Create a mock repository with MockBehavior.Loose to allow setting up only needed methods
            _mockRepository = new Mock<AttendeeRepository>(MockBehavior.Loose, null);
            
            // Create the controller with the mock repository
            _controller = new AttendeeController(_mockRepository.Object);
        }

        [Fact]
        public void GetAttendee_ReturnsOk_WhenAttendeeExists()
        {
            // Arrange
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
            
            // Setup mock repository to return the test attendee
            _mockRepository.Setup(repo => repo.GetAttendeeById("TEST01"))
                .Returns(testAttendee);
            
            // Act
            var result = _controller.GetAttendee("TEST01");
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAttendee = Assert.IsType<Attendee>(okResult.Value);
            Assert.Equal("TEST01", returnedAttendee.attendeeId);
            Assert.Equal("Test", returnedAttendee.firstName);
        }
        
        [Fact]
        public void GetAttendee_ReturnsNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange - Setup mock to return null for non-existent attendee
            _mockRepository.Setup(repo => repo.GetAttendeeById("NONEXISTENT"))
                .Returns((Attendee)null);
            
            // Act
            var result = _controller.GetAttendee("NONEXISTENT");
            
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void GetAttendees_ReturnsOk_WithListOfAttendees()
        {
            // Arrange
            var attendees = new List<Attendee>
            {
                new Attendee { attendeeId = "AT001", firstName = "John", lastName = "Doe" },
                new Attendee { attendeeId = "AT002", firstName = "Jane", lastName = "Smith" }
            };
            
            _mockRepository.Setup(repo => repo.GetAttendees())
                .Returns(attendees);
                
            // Act
            var result = _controller.GetAttendees();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAttendees = Assert.IsAssignableFrom<IEnumerable<Attendee>>(okResult.Value);
            Assert.Equal(2, ((List<Attendee>)returnedAttendees).Count);
        }
        
        [Fact]
        public void Post_ReturnsOk_WhenAttendeeIsValid()
        {
            // Arrange
            var newAttendee = new Attendee
            {
                attendeeId = "NEW001",
                eventId = 1,
                firstName = "New",
                lastName = "User",
                email = "new@example.com"
            };
            
            _mockRepository.Setup(repo => repo.InsertAttendee(It.IsAny<Attendee>()))
                .Returns(true);
                
            // Act
            var result = _controller.Post(newAttendee);
            
            // Assert
            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public void Post_ReturnsBadRequest_WhenAttendeeIsInvalid()
        {
            // Arrange - Create an invalid attendee (missing required fields)
            var invalidAttendee = new Attendee
            {
                // Missing firstName, lastName, and email
                attendeeId = "NEW001"
            };
            
            // Act
            var result = _controller.Post(invalidAttendee);
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void UpdateAttendee_ReturnsOk_WhenAttendeeExistsAndIsValid()
        {
            // Arrange
            var existingAttendee = new Attendee
            {
                attendeeId = "AT001",
                eventId = 1,
                firstName = "Updated",
                lastName = "User",
                email = "updated@example.com"
            };
            
            _mockRepository.Setup(repo => repo.GetAttendeeById("AT001"))
                .Returns(existingAttendee);
                
            _mockRepository.Setup(repo => repo.UpdateAttendee(It.IsAny<Attendee>()))
                .Returns(true);
                
            // Act
            var result = _controller.UpdateAttendee("AT001", existingAttendee);
            
            // Assert
            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public void UpdateAttendee_ReturnsNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var nonExistentAttendee = new Attendee
            {
                attendeeId = "NONEXISTENT",
                firstName = "Updated",
                lastName = "User",
                email = "updated@example.com"
            };
            
            _mockRepository.Setup(repo => repo.GetAttendeeById("NONEXISTENT"))
                .Returns((Attendee)null);
                
            // Act
            var result = _controller.UpdateAttendee("NONEXISTENT", nonExistentAttendee);
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public void DeleteAttendee_ReturnsNoContent_WhenAttendeeExists()
        {
            // Arrange
            var existingAttendee = new Attendee { attendeeId = "AT001" };
            
            _mockRepository.Setup(repo => repo.GetAttendeeById("AT001"))
                .Returns(existingAttendee);
                
            _mockRepository.Setup(repo => repo.DeleteAttendee("AT001"))
                .Returns(true);
                
            // Act
            var result = _controller.DeleteAttendee("AT001");
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public void DeleteAttendee_ReturnsNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAttendeeById("NONEXISTENT"))
                .Returns((Attendee)null);
                
            // Act
            var result = _controller.DeleteAttendee("NONEXISTENT");
            
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public void DeleteAttendee_ReturnsBadRequest_WhenDeletionFails()
        {
            // Arrange
            var existingAttendee = new Attendee { attendeeId = "AT001" };
            
            _mockRepository.Setup(repo => repo.GetAttendeeById("AT001"))
                .Returns(existingAttendee);
                
            // Setup the delete to fail
            _mockRepository.Setup(repo => repo.DeleteAttendee("AT001"))
                .Returns(false);
                
            // Act
            var result = _controller.DeleteAttendee("AT001");
            
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}