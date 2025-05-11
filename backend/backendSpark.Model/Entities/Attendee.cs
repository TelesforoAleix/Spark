namespace backendSpark.Model.Entities;

public class Attendee {
    public Attendee(string attendeeId){
        this.attendeeId = attendeeId;}

    public Attendee(){}
        

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

