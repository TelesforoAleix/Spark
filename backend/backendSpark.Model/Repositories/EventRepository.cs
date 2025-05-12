using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace backendSpark.Model.Repositories;

// Purpose: This class represents the repository for events. It contains methods to interact with the event table in the database.
public class EventRepository : BaseRepository
{
    public EventRepository(IConfiguration configuration) : base(configuration)
    {
    }

    // GetEvents: Retrieves all events from the database. It creates a new NpgsqlConnection, executes a SQL command to select all events, and returns a list of Event objects.
    public virtual List<Event> GetEvents()
    {
        NpgsqlConnection dbConn = null;
        var events = new List<Event>();
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from event";
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                while (data.Read())
                {
                    Event e = new Event(Convert.ToInt32(data["event_id"]))
                    {
                        Name = data["name"].ToString(),
                        StartDate = Convert.ToDateTime(data["startdate"]),
                        FinishDate = Convert.ToDateTime(data["finishdate"]),
                        Location = data["location"].ToString(),
                        Bio = data["bio"].ToString(),
                        NetworkingStartDate = Convert.ToDateTime(data["networking_startdate"]),
                        NetworkingFinishDate = Convert.ToDateTime(data["networking_finishdate"]),
                        MeetingDuration = Convert.ToInt32(data["meeting_duration"]),
                        BreakDuration = Convert.ToInt32(data["break_duration"]),
                        AvailableTables = Convert.ToInt32(data["available_tables"])
                    };
                    events.Add(e);
                }
            }
            return events;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    // GetEventById: Retrieves an event by its ID from the database. It creates a new NpgsqlConnection, executes a SQL command to select the event, and returns an Event object if found.
    public virtual Event GetEventById(int event_id)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "select * from event where event_id = @event_id";
            cmd.Parameters.Add("@event_id", NpgsqlDbType.Integer).Value = event_id;
            var data = GetData(dbConn, cmd);
            if (data != null)
            {
                if (data.Read())
                {
                    return new Event(Convert.ToInt32(data["event_id"]))
                    {
                        Name = data["name"].ToString(),
                        StartDate = Convert.ToDateTime(data["startdate"]),
                        FinishDate = Convert.ToDateTime(data["finishdate"]),
                        Location = data["location"].ToString(),
                        Bio = data["bio"].ToString(),
                        NetworkingStartDate = Convert.ToDateTime(data["networking_startdate"]),
                        NetworkingFinishDate = Convert.ToDateTime(data["networking_finishdate"]),
                        MeetingDuration = Convert.ToInt32(data["meeting_duration"]),
                        BreakDuration = Convert.ToInt32(data["break_duration"]),
                        AvailableTables = Convert.ToInt32(data["available_tables"])
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

    // UpdateEvent: Updates an event in the database. It creates a new NpgsqlConnection, executes a SQL command to update the event, and returns true if successful.
    public virtual bool UpdateEvent(Event e)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
    update event set
    name = @name,
    startdate = @startdate,
    finishdate = @finishdate,
    location = @location,
    bio = @bio,
    networking_startdate = @networkingstartdate,
    networking_finishdate = @networkingfinishdate,
    meeting_duration = @meetingduration,
    break_duration = @breakduration,
    available_tables = @availabletables
    where
    event_id = @event_id";
        cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Integer, e.EventId);
        cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, e.Name);
        cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, e.StartDate);
        cmd.Parameters.AddWithValue("@finishdate", NpgsqlDbType.Timestamp, e.FinishDate);
        cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, e.Location);
        cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, e.Bio);
        cmd.Parameters.AddWithValue("@networkingstartdate", NpgsqlDbType.Timestamp, e.NetworkingStartDate);
        cmd.Parameters.AddWithValue("@networkingfinishdate", NpgsqlDbType.Timestamp, e.NetworkingFinishDate);
        cmd.Parameters.AddWithValue("@meetingduration", NpgsqlDbType.Integer, e.MeetingDuration);
        cmd.Parameters.AddWithValue("@breakduration", NpgsqlDbType.Integer, e.BreakDuration);
        cmd.Parameters.AddWithValue("@availabletables", NpgsqlDbType.Integer, e.AvailableTables);
        bool result = UpdateData(dbConn, cmd);
        return result;
    }

}

// Notes to the code:
    // All functions are virtual so that they can be overridden in the test project.

