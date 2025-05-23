using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
namespace backendSpark.Model.Repositories;

// Purpose: Contains the AttendeeRepository class which is responsible for interacting with the attendee table in the database. It contains methods to get, insert, update, and delete attendees. 
// The AttendeeRepository class inherits from the BaseRepository class, which contains common database operations.
public class AttendeeRepository : BaseRepository
{
    
    public AttendeeRepository(IConfiguration configuration) : base(configuration) { }

    // GetAttendeeById: Retrieves an attendee by their ID from the database. It creates a new NpgsqlConnection, executes a SQL command to select the attendee, and returns an Attendee object if found.
    public virtual Attendee GetAttendeeById(string attendeeId)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            //create a new connection for database 
            dbConn = new NpgsqlConnection(ConnectionString);
            //creating an SQL command 
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from attendee where attendee_id = @attendee_id";
            cmd.Parameters.Add("@attendee_id", NpgsqlDbType.Varchar).Value = attendeeId;
            //call the base method to get data 
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                if (data.Read()) //every time loop runs it reads next like from fetched rows 
                {
                    return new Attendee(data["attendee_id"].ToString())
                    {
                        attendeeId = data["attendee_id"].ToString(),
                        eventId = Convert.ToInt32(data["event_id"]),
                        firstName = data["first_name"].ToString(),
                        lastName = data["last_name"].ToString(),
                        email = data["email"].ToString(),
                        hashed_password = data["hashed_password"].ToString(),
                        header = data["header"].ToString(),
                        bio = data["bio"].ToString(),
                        link = data["link"].ToString()
                    };
                }
            }
            return null;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    // GetAttendees: Retrieves all attendees from the database. It creates a new NpgsqlConnection, executes a SQL command to select all attendees, and returns a list of Attendee objects.
    public virtual List<Attendee> GetAttendees()
    {
        NpgsqlConnection dbConn = null;
        var attendees = new List<Attendee>();
        try
        {
            //create a new connection for database 
            dbConn = new NpgsqlConnection(ConnectionString);
            //creating an SQL command 
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from attendee";
            //call the base method to get data 
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read()) //every time loop runs it reads next like from fetched rows 
                {
                    Attendee a = new Attendee(data["attendee_id"].ToString())
                    {
                        attendeeId = data["attendee_id"].ToString(),
                        eventId = Convert.ToInt32(data["event_id"]),
                        firstName = data["first_name"].ToString(),
                        lastName = data["last_name"].ToString(),
                        email = data["email"].ToString(),
                        hashed_password = data["hashed_password"].ToString(),
                        header = data["header"].ToString(),
                        bio = data["bio"].ToString(),
                        link = data["link"].ToString()
                    };
                    attendees.Add(a);
                }
            }
            return attendees;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    // InsertAttendee: Inserts a new attendee into the database. It creates a new NpgsqlConnection, prepares an SQL command with parameters, and executes the command to insert the attendee.
    public virtual bool InsertAttendee(Attendee a)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @" 
        insert into attendee (attendee_id, event_id, first_name, last_name, email, hashed_password, header, bio, link)
        values 
        (@attendee_id, @event_id, @first_name, @last_name, @email, @hashed_password, @header, @bio, @link)";

            //adding parameters in a better way 
            cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Text, a.attendeeId);
            cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Integer, a.eventId);
            cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, a.firstName);
            cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, a.lastName);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, a.email);
            cmd.Parameters.AddWithValue("@hashed_password", NpgsqlDbType.Text, a.hashed_password);
            cmd.Parameters.AddWithValue("@header", NpgsqlDbType.Text, a.header);
            cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, a.bio);
            cmd.Parameters.AddWithValue("@link", NpgsqlDbType.Text, a.link);

            //will return true if all goes well 
            bool result = InsertData(dbConn, cmd);
            return result;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    // UpdateAttendee: Updates an existing attendee in the database. It creates a new NpgsqlConnection, prepares an SQL command with parameters, and executes the command to update the attendee.
    public virtual bool UpdateAttendee(Attendee a)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @" update attendee set
    event_id=@event_id,
    first_name=@first_name,
    last_name=@last_name,
    email=@email,
    hashed_password=@hashed_password,
    header=@header,
    bio=@bio,
    link=@link
    where
    attendee_id = @attendee_id";
        cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Text, a.attendeeId);
        cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Integer, a.eventId);
        cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, a.firstName);
        cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, a.lastName);
        cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, a.email);
        cmd.Parameters.AddWithValue("@hashed_password", NpgsqlDbType.Text, a.hashed_password);
        cmd.Parameters.AddWithValue("@header", NpgsqlDbType.Text, a.header);
        cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, a.bio);
        cmd.Parameters.AddWithValue("@link", NpgsqlDbType.Text, a.link);
        bool result = UpdateData(dbConn, cmd);
        return result;
    } 
    
    // DeleteAttendee: Deletes an attendee from the database by their ID. It creates a new NpgsqlConnection, prepares an SQL command with parameters, and executes the command to delete the attendee.
    public virtual bool DeleteAttendee(string attendeeId)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = "delete from attendee where attendee_id = @attendee_id";
        cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Varchar, attendeeId);
        bool result = DeleteData(dbConn, cmd);
        return result;
    }

}

// Notes to the code:
    // All functions are virtual so that they can be overridden in the test project.