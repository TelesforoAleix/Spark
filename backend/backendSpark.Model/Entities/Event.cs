using System;
namespace backendSpark.Model.Entities;
public class Event
{
    public Event(int id) { this.EventId = id; }
    public int EventId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public string Location { get; set; }
    public string Bio { get; set; }
    public DateTime NetworkingStartDate { get; set; }
    public DateTime NetworkingFinishDate { get; set; }
    public int MeetingDuration { get; set; }
    public int BreakDuration { get; set; }
    public int AvailableTables { get; set; }

}