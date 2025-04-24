// Purpose: Exposes API endpoints for handling Meeting data via HTTP requests.
//  Endpoints: 
// - GET /api/meeting
// - GET /api/meeting/attendee/{id}
// - GET /api/meeting/table/{table}
// - POST /api/meeting

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

        [HttpGet]
        public ActionResult<List<Meeting>> GetAll()
        {
            return Ok(_repository.GetAllMeetings());
        }

        /* [HttpGet("attendee/{id}")]
        public ActionResult<List<Meeting>> GetByAttendee(int id)
        {
            return Ok(_repository.GetMeetingsByAttendee(id));
        }

        [HttpGet("table/{table}")]
        public ActionResult<List<Meeting>> GetByTable(string table)
        {
            return Ok(_repository.GetMeetingsByTable(table));
        }  */

        [HttpPost]
        public IActionResult CreateMeeting([FromBody] Meeting meeting)
        {
            var success = _repository.InsertMeeting(meeting);
            return success ? Ok() : BadRequest("Could not insert meeting");
        }
    }
}
