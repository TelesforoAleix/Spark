using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace backendSpark.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 

    // This controller handles all operations related to attendees, including CRUD operations.
    // It uses the AttendeeRepository to interact with the data layer.
    // The controller is responsible for handling HTTP requests and returning appropriate responses.
    public class AttendeeController : ControllerBase 
    { 
        protected AttendeeRepository Repository {get;} 
        public AttendeeController(AttendeeRepository repository) { 
            Repository = repository; 
        } 

        // GET api/attendee/{attendeeId}
        // This method retrieves an attendee by their ID.  
        [HttpGet("{attendeeId}")] 
        public ActionResult<Attendee> GetAttendee([FromRoute] string attendeeId) 
        { 
            Attendee attendee = Repository.GetAttendeeById(attendeeId); 
            if (attendee == null) { 
                return NotFound(); 
            } 
            return Ok(attendee); 
        } 

        // GET api/attendee 
        // This method retrieves all attendees.
        // It returns a list of attendees.
        [HttpGet] 
        public ActionResult<IEnumerable<Attendee>> GetAttendees() 
        { 
            return Ok(Repository.GetAttendees()); 
        } 

        // POST api/attendee
        // This method creates a new attendee.
        // It takes an Attendee object as input and returns a status code based on the operation's success.
        [HttpPost] 
        public ActionResult Post([FromBody] Attendee attendee) { 
            if (attendee == null || string.IsNullOrEmpty(attendee.attendeeId) || 
                string.IsNullOrEmpty(attendee.firstName) || 
                string.IsNullOrEmpty(attendee.lastName) ||
                string.IsNullOrEmpty(attendee.email)) 
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

        // PUT api/attendee/{attendeeId}
        // This method updates an existing attendee.
        // It takes an attendee ID and an Attendee object as input.
        [HttpPut("{attendeeId}")] 
        public ActionResult UpdateAttendee([FromRoute] string attendeeId, [FromBody] Attendee attendee) 
        { 
            if (attendee == null || string.IsNullOrEmpty(attendee.attendeeId) || 
                string.IsNullOrEmpty(attendee.firstName) || 
                string.IsNullOrEmpty(attendee.lastName) ||
                string.IsNullOrEmpty(attendee.email)) 
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
        
        // DELETE api/attendee/{attendeeId}
        // This method deletes an attendee by their ID.
        [HttpDelete("{attendeeId}")] 
        public ActionResult DeleteAttendee([FromRoute] string attendeeId) { 
            Attendee existingAttendee = Repository.GetAttendeeById(attendeeId); 
            if (existingAttendee == null) 
            { 
                return NotFound($"Attendee with Id {attendeeId} not found"); 
            } 
            bool status = Repository.DeleteAttendee(attendeeId); 
            if (status) 
            { 
                return NoContent(); 
            } 
            return BadRequest($"Unable to delete attendee with Id {attendeeId}");         
        } 
    } 
} 