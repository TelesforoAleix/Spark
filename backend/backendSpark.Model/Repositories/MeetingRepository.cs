// Purpose: Provides database methods for the new Meeting entity.
// Methods: GetAllMeetings, GetMeetingsByAttendee, GetMeetingsByTable, InsertMeeting
using System;
using System.Collections.Generic;
using CourseAdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;


namespace CourseAdminSystem.Model.Repositories
{
    public class MeetingRepository : BaseRepository
    {
        public MeetingRepository(IConfiguration configuration) : base(configuration) {}

        public List<Meeting> GetAllMeetings()
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM meeting";
            var reader = GetData(dbConn, cmd);

            while (reader.Read())
            {
                meetings.Add(new Meeting(Convert.ToInt32(reader["id"]))
                {
                    AttendeeId = Convert.ToInt32(reader["attendeeid"]),
                    TableName = reader["tablename"].ToString() ?? "",
                    Time = Convert.ToDateTime(reader["time"])
                });
            }

            return meetings;
        }

        public List<Meeting> GetMeetingsByAttendee(int attendeeId)
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM meeting WHERE attendeeid = @attendeeid";
            cmd.Parameters.AddWithValue("@attendeeid", NpgsqlDbType.Integer, attendeeId);

            var reader = GetData(dbConn, cmd);
            while (reader.Read())
            {
                meetings.Add(new Meeting(Convert.ToInt32(reader["id"]))
                {
                    AttendeeId = attendeeId,
                    TableName = reader["tablename"].ToString() ?? "",
                    Time = Convert.ToDateTime(reader["time"])
                });
            }

            return meetings;
        }

        public List<Meeting> GetMeetingsByTable(string table)
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM meeting WHERE tablename = @table";
            cmd.Parameters.AddWithValue("@table", NpgsqlDbType.Text, table);

            var reader = GetData(dbConn, cmd);
            while (reader.Read())
            {
                meetings.Add(new Meeting(Convert.ToInt32(reader["id"]))
                {
                    AttendeeId = Convert.ToInt32(reader["attendeeid"]),
                    TableName = table,
                    Time = Convert.ToDateTime(reader["time"])
                });
            }

            return meetings;
        }

        public bool InsertMeeting(Meeting m)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO meeting (attendeeid, tablename, time)
                VALUES (@attendeeid, @tablename, @time)";
            cmd.Parameters.AddWithValue("@attendeeid", NpgsqlDbType.Integer, m.AttendeeId);
            cmd.Parameters.AddWithValue("@tablename", NpgsqlDbType.Text, m.TableName);
            cmd.Parameters.AddWithValue("@time", NpgsqlDbType.Timestamp, m.Time);

            return InsertData(dbConn, cmd);
        }
    }
}
