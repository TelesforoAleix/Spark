using backendSpark.Model.Entities; 
using backendSpark.Model.Repositories; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
namespace backendSpark.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class AttendeeController : ControllerBase 
    { 
        protected AttendeeRepository Repository {get;} 
        public AttendeeController(AttendeeRepository repository) { 
            Repository = repository; 
        } 


        [HttpGet("{attendeeId}")] 
        public ActionResult<Attendee> GetAttendee([FromRoute] string attendeeId) 
        { 
            Attendee attendee = Repository.GetAttendeeById(attendeeId); 
            if (attendee == null) { 
                return NotFound(); 
            } 
            return Ok(attendee); 
        } 


        [HttpGet] 
        public ActionResult<IEnumerable<Attendee>> GetAttendees() 
        { 
            return Ok(Repository.GetAttendees()); 
        } 


        [HttpPost] 
        public ActionResult Post([FromBody] Attendee attendee) { 
            if (attendee == null || string.IsNullOrEmpty(attendee.attendeeId) || 
                string.IsNullOrEmpty(attendee.firstName) || 
                string.IsNullOrEmpty(attendee.lastName) ||
                string.IsNullOrEmpty(attendee.email) || 
                string.IsNullOrEmpty(attendee.password)) 
            { 
                return BadRequest("Attendee info not correct"); 
            } 
            bool status = Repository.InsertAttendee(attendee); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Failed to insert attendee."); 
        } 


        [HttpPut("{attendeeId}")] 
        public ActionResult UpdateAttendee([FromRoute] string attendeeId, [FromBody] Attendee attendee) 
        { 
            if (attendee == null || string.IsNullOrEmpty(attendee.attendeeId) || 
                string.IsNullOrEmpty(attendee.firstName) || 
                string.IsNullOrEmpty(attendee.lastName) ||
                string.IsNullOrEmpty(attendee.email) || 
                string.IsNullOrEmpty(attendee.password)) 
            {
                return BadRequest("Attendee info not correct"); 
            } 
            Attendee existinAttendee = Repository.GetAttendeeById(attendee.attendeeId); 
            if (existinAttendee == null) 
            { 
                return NotFound($"Student with orgId {attendee.attendeeId} not found"); 
            } 
            bool status = Repository.UpdateAttendee(attendee); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Something went wrong"); 
        } 
        
        [HttpDelete("{attendeeId}")] 
        public ActionResult DeleteAttendee([FromRoute] string attendeeId) { 
            Attendee existingAttendee = Repository.GetAttendeeById(attendeeId); 
            if (existingAttendee == null) 
            { 
                return NotFound($"Attendee with orgId {attendeeId} not found"); 
            } 
            bool status = Repository.DeleteAttendee(attendeeId); 
            if (status) 
            { 
                return NoContent(); 
            } 
            return BadRequest($"Unable to delete attendee with orgId {attendeeId}");         
        } 
    } 
} 