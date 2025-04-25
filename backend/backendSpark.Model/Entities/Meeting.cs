// Purpose: Represents a scheduled meeting between an attendee and a table at a specific time.

namespace backendSpark.Model.Entities
{
    public class Meeting
    {
        public Meeting(int id) => Id = id;

        public int Id { get; set; }

        public Attendee Attendee1Id { get; set; }
        public Attendee Attendee2Id { get; set; }

        public string TableName { get; set; } = string.Empty;

        public DateTime Time { get; set; }
    }
}