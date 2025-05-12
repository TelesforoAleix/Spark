namespace backendSpark.Model.Entities;

/// Purpose: This class represents an attendee in the system. It contains properties for the attendee's ID, event ID, first name, last name, email, hashed password, header, bio, and link.
public class Attendee {

    // Constructor to initialize an Attendee object with a specific attendeeId.
    public Attendee(string attendeeId){
        this.attendeeId = attendeeId;}

    // Default constructor for creating an Attendee object without any parameters.
    public Attendee(){}
        
    // Constructor to initialize an Attendee object with all properties.
    public Attendee(string attendeeId, string firstName, string lastName, string email, string password, string header, string bio, string link)
    {
        this.attendeeId = attendeeId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.hashed_password = password;
        this.header = header;
        this.bio = bio;
        this.link = link;
    }

    // Properties of the Attendee class.
    public string attendeeId { get; set;} 
    public int  eventId { get; set; } 
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string  hashed_password { get; set; }
    public string header { get; set; }
    public string bio { get; set; }
    public string link { get; set; }
}

