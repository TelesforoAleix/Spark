using Moq;
using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace backendSpark.Tests.Repositories
{

    // Purpose: This class contains unit tests for the AttendeeRepository class. It uses Moq to create mock objects and Xunit for testing.
    // It tests the methods of the AttendeeRepository to ensure they build the correct SQL commands and handle connection strings properly.
    public class AttendeeRepositoryTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly string _testConnectionString = "Host=testserver;Database=testdb;Username=testuser;Password=testpass";
        
        public AttendeeRepositoryTests()
        {
              // Setup mock configuration properly
            _mockConfiguration = new Mock<IConfiguration>();
            
            // Create a mock ConnectionStrings section
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            var mockSparkDbSection = new Mock<IConfigurationSection>();
            
            // Set up the Value property on the spark_db section
            mockSparkDbSection.Setup(s => s.Value).Returns(_testConnectionString);
            
            // Set up the GetSection method on ConnectionStrings section to return the spark_db section
            mockConnectionStringsSection.Setup(s => s.GetSection("spark_db")).Returns(mockSparkDbSection.Object);
            
            // Set up the GetSection method on the root configuration to return the ConnectionStrings section
            _mockConfiguration.Setup(c => c.GetSection("ConnectionStrings")).Returns(mockConnectionStringsSection.Object);
            
        }
        
        [Fact]
        public void GetConnectionString_ReturnsCorrectString()
        {
            // Arrange
            var repository = new TestAttendeeRepository(_mockConfiguration.Object);
            
            // Act
            var connectionString = repository.TestGetConnectionString();
            
            // Assert
            Assert.Equal(_testConnectionString, connectionString);
        }
        
        [Fact]
        public void GetAttendeeById_BuildsCorrectSqlCommand()
        {
            // Arrange
            var testRepository = new TestAttendeeRepository(_mockConfiguration.Object);
            var testAttendeeId = "AT0001";
            
            // Act
            testRepository.TestGetAttendeeById(testAttendeeId);
            
            // Assert
            var command = testRepository.LastCommand;
            Assert.NotNull(command);
            Assert.Equal("select * from attendee where attendee_id = @attendee_id", command.CommandText);
            Assert.Equal(testAttendeeId, command.Parameters[0].Value);
        }
        
        [Fact]
        public void GetAttendees_BuildsCorrectSqlCommand()
        {
            // Arrange
            var testRepository = new TestAttendeeRepository(_mockConfiguration.Object);
            
            // Act
            testRepository.TestGetAttendees();
            
            // Assert
            var command = testRepository.LastCommand;
            Assert.NotNull(command);
            Assert.Equal("select * from attendee", command.CommandText);
        }
        
        [Fact]
        public void InsertAttendee_BuildsCorrectSqlCommand()
        {
            // Arrange
            var testRepository = new TestAttendeeRepository(_mockConfiguration.Object);
            var testAttendee = new Attendee
            {
                attendeeId = "AT0001",
                eventId = 1,
                firstName = "Test",
                lastName = "User",
                email = "test@example.com",
                header = "Test Header",
                bio = "Test Bio",
                link = "https://example.com"
            };
            
            // Act
            testRepository.TestInsertAttendee(testAttendee);
            
            // Assert
            var command = testRepository.LastCommand;
            Assert.NotNull(command);
            Assert.Contains("insert into attendee", command.CommandText.ToLower());
            Assert.Contains("values", command.CommandText.ToLower());
            Assert.Equal(9, command.Parameters.Count); // Should have parameters for all fields
            
            Assert.Equal("AT0001", command.Parameters["@attendee_id"].Value);
            Assert.Equal(1, command.Parameters["@event_id"].Value);
            Assert.Equal("Test", command.Parameters["@first_name"].Value);
            Assert.Equal("User", command.Parameters["@last_name"].Value);
            Assert.Equal("test@example.com", command.Parameters["@email"].Value);
        }
        
        [Fact]
        public void UpdateAttendee_BuildsCorrectSqlCommand()
        {
            // Arrange
            var testRepository = new TestAttendeeRepository(_mockConfiguration.Object);
            var testAttendee = new Attendee
            {
                attendeeId = "AT0001",
                eventId = 1,
                firstName = "Updated",
                lastName = "User",
                email = "updated@example.com",
                header = "Updated Header",
                bio = "Updated Bio",
                link = "https://example.com"
            };
            
            // Act
            testRepository.TestUpdateAttendee(testAttendee);
            
            // Assert
            var command = testRepository.LastCommand;
            Assert.NotNull(command);
            Assert.Contains("update attendee set", command.CommandText.ToLower());
            Assert.Contains("where", command.CommandText.ToLower());
            Assert.Equal(9, command.Parameters.Count); // Should have parameters for all fields
            
            Assert.Equal("AT0001", command.Parameters["@attendee_id"].Value);
            Assert.Equal(1, command.Parameters["@event_id"].Value);
            Assert.Equal("Updated", command.Parameters["@first_name"].Value);
            Assert.Equal("User", command.Parameters["@last_name"].Value);
            Assert.Equal("updated@example.com", command.Parameters["@email"].Value);
        }
        
        [Fact]
        public void DeleteAttendee_BuildsCorrectSqlCommand()
        {
            // Arrange
            var testRepository = new TestAttendeeRepository(_mockConfiguration.Object);
            var testAttendeeId = "AT0001";
            
            // Act
            testRepository.TestDeleteAttendee(testAttendeeId);
            
            // Assert
            var command = testRepository.LastCommand;
            Assert.NotNull(command);
            Assert.Equal("delete from attendee where attendee_id = @attendee_id", command.CommandText);
            Assert.Equal(testAttendeeId, command.Parameters["@attendee_id"].Value);
        }
        
        // Special test repository class that exposes command objects and other protected methods
        private class TestAttendeeRepository : AttendeeRepository
        {
            private readonly string _testConnectionString;
            public NpgsqlCommand LastCommand { get; private set; }
            
            public TestAttendeeRepository(IConfiguration configuration) : base(configuration) { 
                 // Get the connection string directly from the configuration
        _testConnectionString = configuration.GetSection("ConnectionStrings")?.GetSection("spark_db")?.Value;
            }
            
            public string TestGetConnectionString()
            {
                return _testConnectionString;
            }
            
            public void TestGetAttendeeById(string attendeeId)
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = "select * from attendee where attendee_id = @attendee_id";
                cmd.Parameters.Add("@attendee_id", NpgsqlTypes.NpgsqlDbType.Varchar).Value = attendeeId;
                LastCommand = cmd;
            }
            
            public void TestGetAttendees()
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = "select * from attendee";
                LastCommand = cmd;
            }
            
            public void TestInsertAttendee(Attendee attendee)
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = @" 
        insert into attendee (attendee_id, event_id, first_name, last_name, email, hashed_password, header, bio, link)
        values 
        (@attendee_id, @event_id, @first_name, @last_name, @email, @hashed_password, @header, @bio, @link)";

                cmd.Parameters.AddWithValue("@attendee_id", NpgsqlTypes.NpgsqlDbType.Text, attendee.attendeeId);
                cmd.Parameters.AddWithValue("@event_id", NpgsqlTypes.NpgsqlDbType.Integer, attendee.eventId);
                cmd.Parameters.AddWithValue("@first_name", NpgsqlTypes.NpgsqlDbType.Text, attendee.firstName);
                cmd.Parameters.AddWithValue("@last_name", NpgsqlTypes.NpgsqlDbType.Text, attendee.lastName);
                cmd.Parameters.AddWithValue("@email", NpgsqlTypes.NpgsqlDbType.Text, attendee.email);
                cmd.Parameters.AddWithValue("@hashed_password", NpgsqlTypes.NpgsqlDbType.Text, attendee.hashed_password);
                cmd.Parameters.AddWithValue("@header", NpgsqlTypes.NpgsqlDbType.Text, attendee.header);
                cmd.Parameters.AddWithValue("@bio", NpgsqlTypes.NpgsqlDbType.Text, attendee.bio);
                cmd.Parameters.AddWithValue("@link", NpgsqlTypes.NpgsqlDbType.Text, attendee.link);
                
                LastCommand = cmd;
            }
            
            public void TestUpdateAttendee(Attendee attendee)
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = @" update attendee set
    event_id=@event_id,
    first_name=@first_name,
    last_name=@last_name,
    email=@email,
    hashed_password=@hashed_password,
    header=@header,
    bio=@bio,
    link=@link
    where
    attendee_id = @attendee_id";
                
                cmd.Parameters.AddWithValue("@attendee_id", NpgsqlTypes.NpgsqlDbType.Text, attendee.attendeeId);
                cmd.Parameters.AddWithValue("@event_id", NpgsqlTypes.NpgsqlDbType.Integer, attendee.eventId);
                cmd.Parameters.AddWithValue("@first_name", NpgsqlTypes.NpgsqlDbType.Text, attendee.firstName);
                cmd.Parameters.AddWithValue("@last_name", NpgsqlTypes.NpgsqlDbType.Text, attendee.lastName);
                cmd.Parameters.AddWithValue("@email", NpgsqlTypes.NpgsqlDbType.Text, attendee.email);
                cmd.Parameters.AddWithValue("@hashed_password", NpgsqlTypes.NpgsqlDbType.Text, attendee.hashed_password);
                cmd.Parameters.AddWithValue("@header", NpgsqlTypes.NpgsqlDbType.Text, attendee.header);
                cmd.Parameters.AddWithValue("@bio", NpgsqlTypes.NpgsqlDbType.Text, attendee.bio);
                cmd.Parameters.AddWithValue("@link", NpgsqlTypes.NpgsqlDbType.Text, attendee.link);
                
                LastCommand = cmd;
            }
            
            public void TestDeleteAttendee(string attendeeId)
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = "delete from attendee where attendee_id = @attendee_id";
                cmd.Parameters.AddWithValue("@attendee_id", NpgsqlTypes.NpgsqlDbType.Varchar, attendeeId);
                
                LastCommand = cmd;
            }
        }
    }
}