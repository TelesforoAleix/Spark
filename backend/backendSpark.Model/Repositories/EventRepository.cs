using System;
using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace backendSpark.Model.Repositories;

public class EventRepository : BaseRepository
{
    public EventRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public Event GetEventById(int event_id)
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
                        NumberOfMaxAttendee = Convert.ToInt32(data["numberofmaxattendee"]),
                        NumberOfTables = Convert.ToInt32(data["numberoftables"])
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

    public List<Event> GetEvents()
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
                        NumberOfMaxAttendee = Convert.ToInt32(data["numberofmaxattendee"]),
                        NumberOfTables = Convert.ToInt32(data["numberoftables"])
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

    public bool InsertEvent(Event e)
    {
        NpgsqlConnection dbConn = null;
        try
        {
            dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
insert into event
(name, startdate, finishdate, location, bio, numberofmaxattendee, numberoftables)
values
(@name, @startdate, @finishdate, @location, @bio, @numberofmaxattendee, @numberoftables)
";
            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, e.Name);
            cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Date, e.StartDate);
            cmd.Parameters.AddWithValue("@finishdate", NpgsqlDbType.Date, e.FinishDate);
            cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, e.Location);
            cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, e.Bio);
            cmd.Parameters.AddWithValue("@numberofmaxattendee", NpgsqlDbType.Integer, e.NumberOfMaxAttendee);
            cmd.Parameters.AddWithValue("@numberoftables", NpgsqlDbType.Integer, e.NumberOfTables);
            bool result = InsertData(dbConn, cmd);
            return result;
        }
        finally
        {
            dbConn?.Close();
        }
    }

    public bool UpdateEvent(Event e)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
update event set
    name=@name,
    startdate=@startdate,
    finishdate=@finishdate,
    location=@location,
    bio=@bio,
    numberofmaxattendee=@numberofmaxattendee,
    numberoftables=@numberoftables
where
event_id = @event_id";
        cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, e.Name);
        cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Date, e.StartDate);
        cmd.Parameters.AddWithValue("@finishdate", NpgsqlDbType.Date, e.FinishDate);
        cmd.Parameters.AddWithValue("@location", NpgsqlDbType.Text, e.Location);
        cmd.Parameters.AddWithValue("@bio", NpgsqlDbType.Text, e.Bio);
        cmd.Parameters.AddWithValue("@numberofmaxattendee", NpgsqlDbType.Integer, e.NumberOfMaxAttendee);
        cmd.Parameters.AddWithValue("@numberoftables", NpgsqlDbType.Integer, e.NumberOfTables);
        cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Integer, e.Id);
        bool result = UpdateData(dbConn, cmd);
        return result;
    }

    public bool DeleteEvent(int event_id)
    {
        var dbConn = new NpgsqlConnection(ConnectionString);
        var cmd = dbConn.CreateCommand();
        cmd.CommandText = @"
delete from event
where event_id = @event_id
";
        cmd.Parameters.AddWithValue("@event_id", NpgsqlDbType.Integer, event_id);
        bool result = DeleteData(dbConn, cmd);
        return result;
    }
}

