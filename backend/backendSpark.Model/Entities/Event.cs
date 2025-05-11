using System.Text.Json.Serialization;

namespace backendSpark.Model.Entities;
public class Event
{
    public Event() { }

    public Event(int id) { this.EventId = id; }
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