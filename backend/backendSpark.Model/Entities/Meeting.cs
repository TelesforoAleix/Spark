// Purpose: Represents a scheduled meeting between an attendee and a table at a specific time.

namespace CourseAdminSystem.Model.Entities
{
    public class Meeting
    {
        public Meeting(int id) => Id = id;

        public int Id { get; set; }

        public int AttendeeId { get; set; }

        public string TableName { get; set; } = string.Empty;

        public DateTime Time { get; set; }
    }
}
