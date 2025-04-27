using System;
using System.Collections.Generic;
using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace backendSpark.Model.Repositories
{
    public class MeetingRepository : BaseRepository
    {
        private readonly AttendeeRepository _attendeeRepository;

        public MeetingRepository(IConfiguration configuration, AttendeeRepository attendeeRepository)
            : base(configuration)
        {
            _attendeeRepository = attendeeRepository;
        }

        // Get all meetings
        public List<Meeting> GetAllMeetings()
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM appointment 
                ORDER BY date, start_time";

            var reader = GetData(dbConn, cmd);

            while (reader.Read())
            {
                meetings.Add(MapMeetingFromReader(reader));
            }

            return meetings;
        }

        // Get meetings by attendee ID
        public List<Meeting> GetMeetingsByAttendee(int attendeeId)
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM appointment 
                WHERE attendee1_id = @attendeeId OR attendee2_id = @attendeeId
                ORDER BY date, start_time";

            cmd.Parameters.AddWithValue("@attendeeId", NpgsqlDbType.Integer, attendeeId);

            var reader = GetData(dbConn, cmd);

            while (reader.Read())
            {
                meetings.Add(MapMeetingFromReader(reader));
            }

            return meetings;
        }

        // Get meetings by table name
        public List<Meeting> GetMeetingsByTable(string tableName)
        {
            var meetings = new List<Meeting>();
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM appointment 
                WHERE table_name = @tableName
                ORDER BY date, start_time";

            cmd.Parameters.AddWithValue("@tableName", NpgsqlDbType.Text, tableName);

            var reader = GetData(dbConn, cmd);

            while (reader.Read())
            {
                meetings.Add(MapMeetingFromReader(reader));
            }

            return meetings;
        }

        // Get a single meeting by ID
        public Meeting? GetMeetingById(int id)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * FROM appointment WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            var reader = GetData(dbConn, cmd);

            if (reader.Read())
            {
                return MapMeetingFromReader(reader);
            }

            return null;
        }

        // Insert a new meeting
        public bool InsertMeeting(Meeting meeting)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO appointment 
                (attendee1_id, attendee2_id, table_name, date, start_time, finish_time)
                VALUES 
                (@attendee1Id, @attendee2Id, @tableName, @date, @startTime, @finishTime)";

            cmd.Parameters.AddWithValue("@attendee1Id", NpgsqlDbType.Integer, meeting.Attendee1Id);
            cmd.Parameters.AddWithValue("@attendee2Id", NpgsqlDbType.Integer, meeting.Attendee2Id);
            cmd.Parameters.AddWithValue("@tableName", NpgsqlDbType.Text, meeting.TableName);
            cmd.Parameters.AddWithValue("@date", NpgsqlDbType.Date, meeting.Date);
            cmd.Parameters.AddWithValue("@startTime", NpgsqlDbType.Timestamp, meeting.StartTime);
            cmd.Parameters.AddWithValue("@finishTime", NpgsqlDbType.Timestamp, meeting.FinishTime);

            return InsertData(dbConn, cmd);
        }

        // Update an existing meeting
        public bool UpdateMeeting(Meeting meeting)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE appointment SET
                attendee1_id = @attendee1Id,
                attendee2_id = @attendee2Id,
                table_name = @tableName,
                date = @date,
                start_time = @startTime,
                finish_time = @finishTime
                WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, meeting.Id);
            cmd.Parameters.AddWithValue("@attendee1Id", NpgsqlDbType.Integer, meeting.Attendee1Id);
            cmd.Parameters.AddWithValue("@attendee2Id", NpgsqlDbType.Integer, meeting.Attendee2Id);
            cmd.Parameters.AddWithValue("@tableName", NpgsqlDbType.Text, meeting.TableName);
            cmd.Parameters.AddWithValue("@date", NpgsqlDbType.Date, meeting.Date);
            cmd.Parameters.AddWithValue("@startTime", NpgsqlDbType.Timestamp, meeting.StartTime);
            cmd.Parameters.AddWithValue("@finishTime", NpgsqlDbType.Timestamp, meeting.FinishTime);

            return UpdateData(dbConn, cmd);
        }

        // Delete a meeting
        public bool DeleteMeeting(int id)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM appointment WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd);
        }

        // Delete all meetings (used when generating a new schedule)
        public bool DeleteAllMeetings()
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM appointment";

            return DeleteData(dbConn, cmd);
        }

        // Generate a new schedule for a given event
        public bool GenerateSchedule(int eventId)
        {
            // First, delete all existing meetings
            DeleteAllMeetings();

            // Get event configuration from event repository
            using var eventConn = new NpgsqlConnection(ConnectionString);
            var eventCmd = eventConn.CreateCommand();
            eventCmd.CommandText = "SELECT * FROM event WHERE event_id = @eventId";
            eventCmd.Parameters.AddWithValue("@eventId", NpgsqlDbType.Integer, eventId);

            var eventReader = GetData(eventConn, eventCmd);

            if (!eventReader.Read())
            {
                return false; // Event not found
            }

            var meetingDuration = Convert.ToInt32(eventReader["meeting_duration"]);
            var breakDuration = Convert.ToInt32(eventReader["break_duration"]);
            var availableTables = Convert.ToInt32(eventReader["available_tables"]);
            var networkingStartDate = Convert.ToDateTime(eventReader["networking_startdate"]);
            var networkingFinishDate = Convert.ToDateTime(eventReader["networking_finishdate"]);

            eventReader.Close();
            eventConn.Close();

            // Get all attendees for this event
            using var attendeeConn = new NpgsqlConnection(ConnectionString);
            var attendeeCmd = attendeeConn.CreateCommand();
            attendeeCmd.CommandText = "SELECT * FROM attendee WHERE event_id = @eventId";
            attendeeCmd.Parameters.AddWithValue("@eventId", NpgsqlDbType.Integer, eventId);

            var attendeeReader = GetData(attendeeConn, attendeeCmd);

            var attendees = new List<int>();
            while (attendeeReader.Read())
            {
                attendees.Add(Convert.ToInt32(attendeeReader["attendee_id"]));
            }

            attendeeReader.Close();
            attendeeConn.Close();

            // If we have fewer than 2 attendees, we can't create meetings
            if (attendees.Count < 2)
            {
                return false;
            }

            // Simple round-robin algorithm for meetings
            var meetings = new List<Meeting>();
            var currentDate = networkingStartDate.Date;
            var currentTime = new DateTime(
                currentDate.Year,
                currentDate.Month,
                currentDate.Day,
                networkingStartDate.Hour,
                networkingStartDate.Minute,
                0);

            // Shuffle attendees to create random pairings
            Random rnd = new Random();
            for (int i = attendees.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                int temp = attendees[i];
                attendees[i] = attendees[j];
                attendees[j] = temp;
            }

            // Create meetings until we reach the networking end date
            while (currentDate <= networkingFinishDate.Date)
            {
                // For each table
                for (int tableIndex = 0; tableIndex < availableTables; tableIndex++)
                {
                    string tableName = $"Table {tableIndex + 1}";

                    // For each possible pair of attendees
                    for (int i = 0; i < attendees.Count; i++)
                    {
                        for (int j = i + 1; j < attendees.Count; j++)
                        {
                            // Check if we've gone past the networking end time
                            var meetingEnd = currentTime.AddMinutes(meetingDuration);

                            if (currentDate > networkingFinishDate.Date ||
                                (currentDate == networkingFinishDate.Date && meetingEnd > networkingFinishDate))
                            {
                                break;
                            }

                            // Create a new meeting
                            var meeting = new Meeting
                            {
                                Attendee1Id = attendees[i].ToString(),
                                Attendee2Id = attendees[j].ToString(),
                                TableName = tableName,
                                Date = currentDate,
                                StartTime = currentTime,
                                FinishTime = meetingEnd
                            };

                            meetings.Add(meeting);

                            // Move to the next time slot
                            currentTime = meetingEnd.AddMinutes(breakDuration);

                            // If we've gone past the end of the day, move to the next day
                            if (currentTime.Hour >= 18)  // Assuming meetings end at 6 PM
                            {
                                currentDate = currentDate.AddDays(1);
                                currentTime = new DateTime(
                                    currentDate.Year,
                                    currentDate.Month,
                                    currentDate.Day,
                                    9, 0, 0);  // Assuming meetings start at 9 AM

                                // If we've moved past the networking end date, break
                                if (currentDate > networkingFinishDate.Date)
                                {
                                    break;
                                }
                            }
                        }

                        if (currentDate > networkingFinishDate.Date)
                        {
                            break;
                        }
                    }

                    if (currentDate > networkingFinishDate.Date)
                    {
                        break;
                    }
                }
            }

            // Insert all meetings
            bool success = true;
            foreach (var meeting in meetings)
            {
                success = InsertMeeting(meeting) && success;
            }

            return success;
        }

        // Helper method to map from database reader to Meeting object
        private Meeting MapMeetingFromReader(NpgsqlDataReader reader)
        {
            var meeting = new Meeting(Convert.ToInt32(reader["id"]))
            {
                Attendee1Id = reader["attendee1_id"].ToString(),
                Attendee2Id = reader["attendee2_id"].ToString(),
                TableName = reader["table_name"].ToString() ?? string.Empty,
                Date = Convert.ToDateTime(reader["date"]),
                StartTime = Convert.ToDateTime(reader["start_time"]),
                FinishTime = Convert.ToDateTime(reader["finish_time"])
            };

            // Optionally fetch attendee details if needed
            meeting.Attendee1 = _attendeeRepository.GetAttendeeById(meeting.Attendee1Id.ToString());
            meeting.Attendee2 = _attendeeRepository.GetAttendeeById(meeting.Attendee2Id.ToString());

            return meeting;
        }
    }
}