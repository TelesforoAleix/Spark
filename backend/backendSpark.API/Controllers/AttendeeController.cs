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


        [HttpGet("{AttendeeId}")] 
        public ActionResult<Attendee> GetAttendee([FromRoute] string AttendeeId) 
        { 
            Attendee attendee = Repository.GetAttendeeById(AttendeeId); 
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
            if (attendee == null || string.IsNullOrEmpty(attendee.AttendeeId) || 
                string.IsNullOrEmpty(attendee.FirstName) || 
                string.IsNullOrEmpty(attendee.LastName) ||
                string.IsNullOrEmpty(attendee.Email) || 
                string.IsNullOrEmpty(attendee.Password)) 
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


        [HttpPut] 
        public ActionResult UpdateAttendee([FromBody] Attendee attendee) 
        { 
            if (attendee == null || string.IsNullOrEmpty(attendee.AttendeeId) || 
                string.IsNullOrEmpty(attendee.FirstName) || 
                string.IsNullOrEmpty(attendee.LastName) ||
                string.IsNullOrEmpty(attendee.Email) || 
                string.IsNullOrEmpty(attendee.Password)) 
            {
                return BadRequest("Attendee info not correct"); 
            } 
            Attendee existinAttendee = Repository.GetAttendeeById(attendee.AttendeeId); 
            if (existinAttendee == null) 
            { 
                return NotFound($"Student with orgId {attendee.AttendeeId} not found"); 
            } 
            bool status = Repository.UpdateAttendee(attendee); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Something went wrong"); 
        } 
        
        [HttpDelete("{AttendeeId}")] 
        public ActionResult DeleteAttendee([FromRoute] string AttendeeId) { 
            Attendee existingAttendee = Repository.GetAttendeeById(AttendeeId); 
            if (existingAttendee == null) 
            { 
                return NotFound($"Attendee with orgId {AttendeeId} not found"); 
            } 
            bool status = Repository.DeleteAttendee(AttendeeId); 
            if (status) 
            { 
                return NoContent(); 
            } 
            return BadRequest($"Unable to delete attendee with orgId {AttendeeId}");         
        } 
    } 
} 