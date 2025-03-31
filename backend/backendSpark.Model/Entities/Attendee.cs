namespace backendSpark.Model.Entities;

public class Attendee {
    public Attendee(string attendeeId){
        attendeeId = AttendeeId;}

    public Attendee(){}
        

    public Attendee(string attendeeId, string firstName, string lastName, string email, string password, string header, string bio, string link)
    {
        AttendeeId = attendeeId; 
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Header = header;
        Bio = bio;
        Link = link;
    }

    public string AttendeeId { get; set;} 
    public string  EventId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Header { get; set; }
    public string Bio { get; set; }
    public string Link { get; set; }
}

