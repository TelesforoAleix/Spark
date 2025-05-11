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
        /// Generate a new meeting schedule for an event
        /// This will delete all existing meetings and create a new schedule based on event info and attendees
        /// </summary>
        [HttpPost("GenerateSchedule")]
        public ActionResult GenerateSchedule([FromBody] GenerateScheduleRequest request)
        {
            Console.WriteLine($"GenerateSchedule called with EventId: {request?.EventId}");
            try
            {
                if (request == null || request.EventId <= 0)
                {
                    return BadRequest("Invalid event ID");
                }

                var result = _repository.GenerateSchedule(request.EventId);
                if (result)
                {
                    // Return the newly generated meetings
                    var meetings = _repository.GetAllMeetings();
                    return Ok(new { 
                        message = "Meeting schedule generated successfully",
                        meetingsCount = meetings.Count,
                        meetings = meetings
                    });
                }
                return BadRequest("Failed to generate meeting schedule");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class GenerateScheduleRequest
    {
        public int EventId { get; set; }
    }
}