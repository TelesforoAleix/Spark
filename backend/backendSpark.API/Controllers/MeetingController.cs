using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backendSpark.API.Controllers
{
    // This controller handles all operations related to meetings, including generating schedules.
    // It uses the MeetingRepository to interact with the data layer.
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingRepository _repository;

        public MeetingController(MeetingRepository repository)
        {
            _repository = repository;
        }

        // GET api/meeting
        // This method retrieves all meetings. It returns a list of meetings.
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

        // POST api/meeting/GenerateSchedule
        // This method generates a meeting schedule based on the provided event ID.
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