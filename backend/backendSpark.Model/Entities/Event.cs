namespace backendSpark.Model.Entities; 
public class Event
{
    public Event(int id){Id = id;}
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public string Location { get; set; }
    public string Bio { get; set; }
    public int NumberOfMaxAttendee { get; set; }
    public int NumberOfTables { get; set; }
}
