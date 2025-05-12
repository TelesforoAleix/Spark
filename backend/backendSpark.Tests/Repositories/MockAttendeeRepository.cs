using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backendSpark.Tests.Mocks
{
    /// <summary>
    /// A concrete mock implementation of AttendeeRepository that uses an in-memory collection
    /// instead of a real database connection. Useful for testing the controller without database access.
    /// </summary>
    public class MockAttendeeRepository : AttendeeRepository
    {
        private readonly Dictionary<string, Attendee> _attendees = new Dictionary<string, Attendee>();
        
        public MockAttendeeRepository() 
            : base(GetMockConfiguration())
        {
            // Initialize with some test data
            SeedTestData();
        }
        
        // Create a mock configuration
        private static IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ConnectionStrings:spark_db", "Host=testserver;Database=testdb;Username=testuser;Password=testpass"}
            };
            
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        
        private void SeedTestData()
        {
            // Add test attendees
            var attendee1 = new Attendee
            {
                attendeeId = "AT0001",
                eventId = 1,
                firstName = "Test",
                lastName = "User",
                email = "test@example.com",
                header = "Test Position",
                bio = "This is a test attendee",
                link = "https://example.com"
            };
            
            var attendee2 = new Attendee
            {
                attendeeId = "AT0002",
                eventId = 1,
                firstName = "Jane",
                lastName = "Smith",
                email = "jane@example.com",
                header = "Designer",
                bio = "Creative professional",
                link = "https://design.com"
            };
            
            _attendees.Add(attendee1.attendeeId, attendee1);
            _attendees.Add(attendee2.attendeeId, attendee2);
        }
        
        // Override repository methods to use in-memory collection
        
        public override Attendee GetAttendeeById(string attendeeId)
        {
            if (_attendees.TryGetValue(attendeeId, out var attendee))
            {
                return attendee;
            }
            return null;
        }
        
        public override List<Attendee> GetAttendees()
        {
            return new List<Attendee>(_attendees.Values);
        }
        
        public override bool InsertAttendee(Attendee attendee)
        {
            // Check if attendee already exists
            if (_attendees.ContainsKey(attendee.attendeeId))
            {
                return false;
            }
            
            // Generate ID if not provided
            if (string.IsNullOrEmpty(attendee.attendeeId))
            {
                attendee.attendeeId = $"AT{Guid.NewGuid().ToString().Substring(0, 8)}";
            }
            
            _attendees.Add(attendee.attendeeId, attendee);
            return true;
        }
        
        public override bool UpdateAttendee(Attendee attendee)
        {
            if (!_attendees.ContainsKey(attendee.attendeeId))
            {
                return false;
            }
            
            _attendees[attendee.attendeeId] = attendee;
            return true;
        }
        
        public override bool DeleteAttendee(string attendeeId)
        {
            return _attendees.Remove(attendeeId);
        }
    }
}