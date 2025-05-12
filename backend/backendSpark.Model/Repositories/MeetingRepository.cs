using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace backendSpark.Model.Repositories
{

    // Purpose: This class represents the repository for meetings. It contains methods to interact with the appointment table in the database.
    public class MeetingRepository : BaseRepository
    {
        private readonly AttendeeRepository _attendeeRepository;

        public MeetingRepository(IConfiguration configuration, AttendeeRepository attendeeRepository)
            : base(configuration)
        {
            _attendeeRepository = attendeeRepository;
        }

        // GetAllMeetings: Retrieve all meetings from the database
        public virtual List<Meeting> GetAllMeetings()
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

        // InsertMeeting: Insert a new meeting into the database
        public virtual bool InsertMeeting(Meeting meeting)
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO appointment 
                (attendee1_id, attendee2_id, table_name, date, start_time, finish_time)
                VALUES 
                (@attendee1Id, @attendee2Id, @tableName, @date, @startTime, @finishTime)";

            cmd.Parameters.AddWithValue("@attendee1Id", NpgsqlDbType.Text, meeting.Attendee1Id);
            cmd.Parameters.AddWithValue("@attendee2Id", NpgsqlDbType.Text, meeting.Attendee2Id);
            cmd.Parameters.AddWithValue("@tableName", NpgsqlDbType.Text, meeting.TableName);
            cmd.Parameters.AddWithValue("@date", NpgsqlDbType.Date, meeting.Date);
            cmd.Parameters.AddWithValue("@startTime", NpgsqlDbType.Timestamp, meeting.StartTime);
            cmd.Parameters.AddWithValue("@finishTime", NpgsqlDbType.Timestamp, meeting.FinishTime);

            return InsertData(dbConn, cmd);
        }


        // DeleteAllMeetings: Delete all meetings from the database
        public virtual bool DeleteAllMeetings()
        {
            using var dbConn = new NpgsqlConnection(ConnectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = "DELETE FROM appointment";

            return DeleteData(dbConn, cmd);
        }

        // GenerateSchedule: Matchmaking algorithm to generate a schedule for the event
        // This method will delete all existing meetings, fetch event configuration, and generate a new schedule based on the event's parameters and the list of attendees.
        public virtual bool GenerateSchedule(int eventId)
        {
            Console.WriteLine($"Generating schedule for event ID: {eventId}");

            // First, delete all existing meetings
            DeleteAllMeetings();
            Console.WriteLine("Deleted all existing meetings.");

            // Get event configuration from event repository
            using var eventConn = new NpgsqlConnection(ConnectionString);
            var eventCmd = eventConn.CreateCommand();
            eventCmd.CommandText = "SELECT * FROM event WHERE event_id = @eventId";
            eventCmd.Parameters.AddWithValue("@eventId", NpgsqlDbType.Integer, eventId);

            var eventReader = GetData(eventConn, eventCmd);

            if (!eventReader.Read())
            {
                Console.WriteLine("Event not found.");
                return false; // Event not found
            }

            Console.WriteLine("Fetching event configuration.");
            var meetingDuration = Convert.ToInt32(eventReader["meeting_duration"]);
            var breakDuration = Convert.ToInt32(eventReader["break_duration"]);
            var availableTables = Convert.ToInt32(eventReader["available_tables"]);
            var networkingStartDate = Convert.ToDateTime(eventReader["networking_startdate"]);
            var networkingFinishDate = Convert.ToDateTime(eventReader["networking_finishdate"]);

            eventReader.Close();
            eventConn.Close();

            Console.WriteLine($"Event config: Duration={meetingDuration}min, Break={breakDuration}min, Tables={availableTables}");
            Console.WriteLine($"Networking period: {networkingStartDate} to {networkingFinishDate}");

            // Get all attendees for this event
            using var attendeeConn = new NpgsqlConnection(ConnectionString);
            var attendeeCmd = attendeeConn.CreateCommand();
            attendeeCmd.CommandText = "SELECT * FROM attendee WHERE event_id = @eventId";
            attendeeCmd.Parameters.AddWithValue("@eventId", NpgsqlDbType.Integer, eventId);

            var attendeeReader = GetData(attendeeConn, attendeeCmd);

            var attendees = new List<string>();
            while (attendeeReader.Read())
            {
                attendees.Add(attendeeReader["attendee_id"].ToString());
            }

            attendeeReader.Close();
            attendeeConn.Close();

            Console.WriteLine($"Found {attendees.Count} attendees");

            // If we have fewer than 2 attendees, we can't create meetings
            if (attendees.Count < 2)
            {
                Console.WriteLine("Not enough attendees to create meetings");
                return false;
            }

            // Calculate total available meeting time in minutes
            TimeSpan networkingDuration = networkingFinishDate - networkingStartDate;
            int totalMinutesAvailable = (int)networkingDuration.TotalMinutes;

            // Calculate how many meeting slots we can fit
            int meetingSlotDuration = meetingDuration + breakDuration;
            int totalPossibleSlots = totalMinutesAvailable / meetingSlotDuration;

            Console.WriteLine($"Total networking duration: {totalMinutesAvailable} minutes");
            Console.WriteLine($"Each meeting slot takes {meetingSlotDuration} minutes");
            Console.WriteLine($"Total possible time slots: {totalPossibleSlots}");

            // Generate all time slots for the networking period
            var timeSlots = new List<DateTime>();
            DateTime currentSlot = networkingStartDate;

            while (currentSlot.Add(TimeSpan.FromMinutes(meetingDuration)) <= networkingFinishDate)
            {
                timeSlots.Add(currentSlot);
                currentSlot = currentSlot.AddMinutes(meetingDuration + breakDuration);
            }

            Console.WriteLine($"Generated {timeSlots.Count} time slots");

            // Generate all possible pairs of attendees
            var allPossiblePairs = new List<(string, string)>();
            for (int i = 0; i < attendees.Count; i++)
            {
                for (int j = i + 1; j < attendees.Count; j++)
                {
                    allPossiblePairs.Add((attendees[i], attendees[j]));
                }
            }

            // Shuffle the pairs for randomness
            Random rnd = new Random();
            allPossiblePairs = allPossiblePairs.OrderBy(x => rnd.Next()).ToList();

            Console.WriteLine($"Generated {allPossiblePairs.Count} possible meeting pairs");

            // Track which attendees are busy at which time slots
            var attendeeBusyTimes = new Dictionary<string, HashSet<DateTime>>();
            foreach (var attendee in attendees)
            {
                attendeeBusyTimes[attendee] = new HashSet<DateTime>();
            }

            // Schedule meetings - fill all tables at each time slot before moving to next time slot
            var scheduledMeetings = new List<Meeting>();

            foreach (var timeSlot in timeSlots)
            {
                Console.WriteLine($"Scheduling meetings for time slot: {timeSlot}");

                // For each available table at this time slot
                for (int tableIndex = 0; tableIndex < availableTables; tableIndex++)
                {
                    string tableName = $"Table {tableIndex + 1}";
                    bool pairFound = false;

                    // Find an unscheduled pair for this slot and table
                    foreach (var pair in allPossiblePairs.ToList()) // Use ToList to avoid collection modification issues
                    {
                        string attendee1 = pair.Item1;
                        string attendee2 = pair.Item2;

                        // Check if either attendee is already busy at this time
                        if (attendeeBusyTimes[attendee1].Contains(timeSlot) ||
                            attendeeBusyTimes[attendee2].Contains(timeSlot))
                        {
                            continue; // Skip this pair - one or both attendees are busy
                        }

                        // Create a meeting for this pair at this time and table
                        var meeting = new Meeting
                        {
                            Attendee1Id = attendee1,
                            Attendee2Id = attendee2,
                            TableName = tableName,
                            Date = timeSlot.Date,
                            StartTime = timeSlot,
                            FinishTime = timeSlot.AddMinutes(meetingDuration)
                        };

                        scheduledMeetings.Add(meeting);

                        // Mark both attendees as busy for this time slot
                        attendeeBusyTimes[attendee1].Add(timeSlot);
                        attendeeBusyTimes[attendee2].Add(timeSlot);

                        // Remove this pair from the list of available pairs
                        allPossiblePairs.Remove(pair);

                        Console.WriteLine($"Scheduled meeting: {attendee1} with {attendee2} at {tableName} starting at {timeSlot}");
                        pairFound = true;
                        break;
                    }

                    if (!pairFound)
                    {
                        Console.WriteLine($"No available pairs for table {tableName} at time {timeSlot}");
                    }

                    // If we've used up all pairs, stop scheduling
                    if (allPossiblePairs.Count == 0)
                    {
                        Console.WriteLine("All possible pairs have been scheduled");
                        break;
                    }
                }

                // If we've used up all pairs, stop scheduling
                if (allPossiblePairs.Count == 0)
                {
                    Console.WriteLine("All possible pairs have been scheduled");
                    break;
                }
            }

            Console.WriteLine($"Generated {scheduledMeetings.Count} meetings");
            Console.WriteLine($"Unscheduled pairs: {allPossiblePairs.Count}");

            // Calculate the maximum possible meetings per attendee
            int maxMeetingsPerAttendee = attendees.Count - 1;

            // Calculate actual meetings per attendee
            var meetingsPerAttendee = new Dictionary<string, int>();
            foreach (var attendee in attendees)
            {
                meetingsPerAttendee[attendee] = 0;
            }

            foreach (var meeting in scheduledMeetings)
            {
                meetingsPerAttendee[meeting.Attendee1Id]++;
                meetingsPerAttendee[meeting.Attendee2Id]++;
            }

            // Show stats for each attendee
            Console.WriteLine("Meetings per attendee:");
            foreach (var kvp in meetingsPerAttendee)
            {
                int meetingCount = kvp.Value;
                double percentage = (double)meetingCount / maxMeetingsPerAttendee * 100;
                Console.WriteLine($"  Attendee {kvp.Key}: {meetingCount}/{maxMeetingsPerAttendee} meetings ({percentage:F1}%)");
            }

            // Insert all meetings into the database
            int successCount = 0;
            foreach (var meeting in scheduledMeetings)
            {
                if (InsertMeeting(meeting))
                {
                    successCount++;
                }
                else
                {
                    Console.WriteLine($"Failed to insert meeting: {meeting.Attendee1Id} with {meeting.Attendee2Id} at {meeting.StartTime}");
                }
            }

            Console.WriteLine($"Successfully inserted {successCount} out of {scheduledMeetings.Count} meetings");
            return successCount > 0;
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