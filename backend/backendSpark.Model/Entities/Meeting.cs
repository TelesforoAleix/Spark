// Purpose: Represents a scheduled meeting between two attendees at a specific table and time.
namespace backendSpark.Model.Entities
{
    // Purpose: This class represents a meeting in the system. It contains properties for the meeting's ID, attendee IDs, table name, date, start time, and finish time. It also includes optional navigation properties for the attendees involved in the meeting.
    public class Meeting
    {
        public Meeting() { }
        
        public Meeting(int id) => Id = id;

        public int Id { get; set; }
        
        public string Attendee1Id { get; set; }
        public string Attendee2Id { get; set; }
        
        public string TableName { get; set; } = string.Empty;
        
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        
        // Navigation properties (for entity relationships)
        public Attendee? Attendee1 { get; set; }
        public Attendee? Attendee2 { get; set; }
    }
}