using System.Text.Json.Serialization;

namespace backendSpark.Model.Entities;

// Purpose: This class represents an event in the system. It contains properties for the event's ID, name, location, bio, start date, finish date, networking start and finish dates, meeting duration, break duration, and available tables.
public class Event
{
    // Constructor to initialize an Event object without any parameters.
    public Event() { }

    // Constructor to initialize an Event object with a specific eventId.
    public Event(int id) { this.EventId = id; }

        // JsonPropertyName attribute is used to specify the name of the property when serialized to JSON.
        [JsonPropertyName("eventId")]
        public int EventId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("finishDate")]
        public DateTime FinishDate { get; set; }

        [JsonPropertyName("networkingStartDate")]
        public DateTime NetworkingStartDate { get; set; }

        [JsonPropertyName("networkingFinishDate")]
        public DateTime NetworkingFinishDate { get; set; }

        [JsonPropertyName("meetingDuration")]
        public int MeetingDuration { get; set; }

        [JsonPropertyName("breakDuration")]
        public int BreakDuration { get; set; }

        [JsonPropertyName("availableTables")]
        public int AvailableTables { get; set; }

}