public class Attendee
{
    public Attendee()
    {
        AttendeeId = Guid.NewGuid().ToString();
    }
    public Attendee(string firstName, string lastName, string email, string hashedPassword, string header, string bio, string link)
    {
        AttendeeId = Guid.NewGuid().ToString(); 
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HashedPassword = hashedPassword;
        Header = header;
        Bio = bio;
        Link = link;
    }

    public string AttendeeId { get; } // Read-only
    public string? EventId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Header { get; set; }
    public string Bio { get; set; }
    public string Link { get; set; }
}

