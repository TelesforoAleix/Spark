using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backendSpark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingRepository _repository;

        public MeetingController(MeetingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all meetings
        /// </summary>
        [HttpGet]
        public ActionResult<List<Meeting>> GetAllMeetings()
        {
            try
            {
                return Ok(_repository.GetAllMeetings());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get meeting by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Meeting> GetMeetingById(int id)
        {
            try
            {
                var meeting = _repository.GetMeetingById(id);
                if (meeting == null)
                {
                    return NotFound($"Meeting with ID {id} not found");
                }
                return Ok(meeting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get meetings by attendee ID
        /// </summary>
        [HttpGet("attendee/{attendeeId}")]
        public ActionResult<List<Meeting>> GetMeetingsByAttendee(int attendeeId)
        {
            try
            {
                return Ok(_repository.GetMeetingsByAttendee(attendeeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get meetings by table name
        /// </summary>
        [HttpGet("table/{tableName}")]
        public ActionResult<List<Meeting>> GetMeetingsByTable(string tableName)
        {
            try
            {
                return Ok(_repository.GetMeetingsByTable(tableName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new meeting
        /// </summary>
        [HttpPost]
        public ActionResult CreateMeeting([FromBody] Meeting meeting)
        {
            try
            {
                if (meeting == null)
                {
                    return BadRequest("Meeting data is null");
                }

                var result = _repository.InsertMeeting(meeting);
                if (result)
                {
                    return StatusCode(201, "Meeting created successfully");
                }
                return BadRequest("Failed to create meeting");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing meeting
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateMeeting(int id, [FromBody] Meeting meeting)
        {
            try
            {
                if (meeting == null)
                {
                    return BadRequest("Meeting data is null");
                }

                if (id != meeting.Id)
                {
                    return BadRequest("Meeting ID mismatch");
                }

                var existingMeeting = _repository.GetMeetingById(id);
                if (existingMeeting == null)
                {
                    return NotFound($"Meeting with ID {id} not found");
                }

                var result = _repository.UpdateMeeting(meeting);
                if (result)
                {
                    return Ok("Meeting updated successfully");
                }
                return BadRequest("Failed to update meeting");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a meeting
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteMeeting(int id)
        {
            try
            {
                var existingMeeting = _repository.GetMeetingById(id);
                if (existingMeeting == null)
                {
                    return NotFound($"Meeting with ID {id} not found");
                }

                var result = _repository.DeleteMeeting(id);
                if (result)
                {
                    return Ok("Meeting deleted successfully");
                }
                return BadRequest($"Failed to delete meeting with ID {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate a new meeting schedule for an event
        /// This will delete all existing meetings and create a new schedule
        /// </summary>
        [HttpPost("generate/{eventId}")]
        public ActionResult GenerateSchedule(int eventId)
        {
            try
            {
                var result = _repository.GenerateSchedule(eventId);
                if (result)
                {
                    return Ok("Meeting schedule generated successfully");
                }
                return BadRequest("Failed to generate meeting schedule");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}