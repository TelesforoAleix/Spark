using System;
using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
namespace backendSpark.Model.Repositories;
public class AttendeeRepository : BaseRepository
{
    public AttendeeRepository(IConfiguration configuration) : base(configuration) { }

    public Attendee GetAttendeeById(string attendeeId)
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
                        AttendeeId = data["attendee_id"].ToString(),
                        EventId = data["event_id"].ToString(),
                        FirstName = data["first_name"].ToString(),
                        LastName = data["last_name"].ToString(),
                        Email = data["email"].ToString(),
                        Password = data["hashed_password"].ToString(),
                        Header = data["header"].ToString(),
                        Bio = data["bio"].ToString(),
                        Link = data["link"].ToString()
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

    public List<Attendee> GetAttendees()
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
                        AttendeeId = data["attendee_id"].ToString(),
                        EventId = data["event_id"].ToString(),
                        FirstName = data["first_name"].ToString(),
                        LastName = data["last_name"].ToString(),
                        Email = data["email"].ToString(),
                        Password = data["hashed_password"].ToString(),
                        Header = data["header"].ToString(),
                        Bio = data["bio"].ToString(),
                        Link = data["link"].ToString()
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

    public bool InsertAttendee(Attendee a)
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
            cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Varchar, a.AttendeeId);
            cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Varchar, a.EventId);
            cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, a.FirstName);
            cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, a.LastName);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, a.Email);
            cmd.Parameters.AddWithValue("@hashed_password", NpgsqlDbType.Text, a.Password);
            cmd.Parameters.AddWithValue("@header", NpgsqlDbType.Text, a.Header);
            cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, a.Bio);
            cmd.Parameters.AddWithValue("@link", NpgsqlDbType.Text, a.Link);

            //will return true if all goes well 
            bool result = InsertData(dbConn, cmd);
            return result;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool UpdateAttendee(Attendee a)
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
        cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Varchar, a.AttendeeId);
        cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Varchar, a.EventId);
        cmd.Parameters.AddWithValue("@first_name", NpgsqlDbType.Text, a.FirstName);
        cmd.Parameters.AddWithValue("@last_name", NpgsqlDbType.Text, a.LastName);
        cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, a.Email);
        cmd.Parameters.AddWithValue("@hashed_password", NpgsqlDbType.Text, a.Password);
        cmd.Parameters.AddWithValue("@header", NpgsqlDbType.Text, a.Header);
        cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, a.Bio);
        cmd.Parameters.AddWithValue("@link", NpgsqlDbType.Text, a.Link);
        bool result = UpdateData(dbConn, cmd);
        return result;
    } 
    public bool DeleteAttendee(string attendeeId)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = "delete from attendee where attendee_id = @attendee_id";
        cmd.Parameters.AddWithValue("@attendee_id", NpgsqlDbType.Varchar, attendeeId);
        bool result = DeleteData(dbConn, cmd);
        return result;
    }

}