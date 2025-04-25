// Purpose: Handles 'appointment' table using Meeting model structure
using System;
using System.Collections.Generic;
using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace backendSpark.Model.Repositories
{
    public class MeetingRepository : BaseRepository
    {
        public MeetingRepository(IConfiguration configuration) : base(configuration) {}

        // Fetch all meetings from the 'appointment' table
        public List<Meeting> GetAllMeetings()
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM appointment";
            var reader = GetData(dbConn, cmd);

            while (reader.Read())
            {
                meetings.Add(new Meeting(0) // appt_id is a string, using 0 as placeholder
                {
                    Attendee1Id.attendeeId = int.TryParse(reader["attendee1_id"]?.ToString(),,
                    Attendee2Id.attendeeId = int.TryParse(reader["attendee2_id"]?.ToString(),
                    TableName = reader["meeting_desk"].ToString() ?? "Unknown",
                    Time = DateTime.Now // Placeholder, as the table doesn't have a time column
                });
            }

            return meetings;
        }

        // Insert a new meeting
        public bool InsertMeeting(Meeting m)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO appointment (attendee1_id, meeting_desk)
                VALUES (@attendeeid, @desk)";
            cmd.Parameters.AddWithValue("@attendeeid", m.AttendeeId.ToString());
            cmd.Parameters.AddWithValue("@desk", m.TableName);

            return InsertData(dbConn, cmd);
        }
    }
}