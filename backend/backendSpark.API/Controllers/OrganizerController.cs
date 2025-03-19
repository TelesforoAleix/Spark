using backendSpark.Model.Entities; 
using backendSpark.Model.Repositories; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
namespace backendSpark.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class OrganizerController : ControllerBase 
    { 
        protected OrganizerRepository Repository {get;} 
        public OrganizerController(OrganizerRepository repository) { 
            Repository = repository; 
        } 
        [HttpGet("{orgId}")] 
        public ActionResult<Organizer> GetOrganizer([FromRoute] string orgId) 
        { 
            Organizer organizer = Repository.GetOrganizerById(orgId); 
            if (organizer == null) { 
                return NotFound(); 
            } 
            return Ok(organizer); 
        } 
        [HttpGet] 
        public ActionResult<IEnumerable<Organizer>> GetOrganizers() 
        { 
            return Ok(Repository.GetOrganizers()); 
        } 
        [HttpPost] 
        public ActionResult Post([FromBody] Organizer organizer) { 
            if (organizer == null || string.IsNullOrEmpty(organizer.OrgId) || 
                string.IsNullOrEmpty(organizer.Name) || 
                string.IsNullOrEmpty(organizer.Email) || 
                string.IsNullOrEmpty(organizer.Password)) 
            { 
                return BadRequest("Organizer info not correct"); 
            } 
            bool status = Repository.InsertOrganizer(organizer); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Failed to insert organizer."); 
        } 
        [HttpPut] 
        public ActionResult UpdateOrganizer([FromBody] Organizer organizer) 
        { 
            if (organizer == null || string.IsNullOrEmpty(organizer.OrgId) || 
                string.IsNullOrEmpty(organizer.Name) || 
                string.IsNullOrEmpty(organizer.Email) || 
                string.IsNullOrEmpty(organizer.Password)) 
            {
                return BadRequest("Organizer info not correct"); 
            } 
            Organizer existinOrganizer = Repository.GetOrganizerById(organizer.OrgId); 
            if (existinOrganizer == null) 
            { 
                return NotFound($"Student with orgId {organizer.OrgId} not found"); 
            } 
            bool status = Repository.UpdateOrganizer(organizer); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Something went wrong"); 
        } 
        [HttpDelete("{orgId}")] 
        public ActionResult DeleteOrganizer([FromRoute] string orgId) { 
            Organizer existingOrganizer = Repository.GetOrganizerById(orgId); 
            if (existingOrganizer == null) 
            { 
                return NotFound($"Organizer with orgId {orgId} not found"); 
            } 
            bool status = Repository.DeleteOrganizer(orgId); 
            if (status) 
            { 
                return NoContent(); 
            } 
            return BadRequest($"Unable to delete organizer with orgId {orgId}");         
        } 
    } 
} 

// EXAMPLE CODE PROVIDED IN CLASS

/*

using CourseAdminSystem.Model.Entities; 
using CourseAdminSystem.Model.Repositories; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
namespace CourseAdminSystem.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class StudentController : ControllerBase 
    { 
        protected StudentRepository Repository {get;} 
        public StudentController(StudentRepository repository) { 
            Repository = repository; 
        } 
        [HttpGet("{id}")] 
        public ActionResult<Student> GetStudent([FromRoute] int id) 
        { 
            Student student = Repository.GetStudentById(id); 
            if (student == null) { 
                return NotFound(); 
            } 
            return Ok(student); 
        } 
        [HttpGet] 
        public ActionResult<IEnumerable<Student>> GetStudents() 
        { 
            return Ok(Repository.GetStudents()); 
        } 
        [HttpPost] 
        public ActionResult Post([FromBody] Student student) { 
            if (student == null) 
            { 
                return BadRequest("Student info not correct"); 
            } 
            bool status = Repository.InsertStudent(student); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest(); 
        } 
        [HttpPut] 
        public ActionResult UpdateStudent([FromBody] Student student) 
        { 
            if (student == null) 
            { 
                return BadRequest("Student info not correct"); 
            } 
            Student existinStudent = Repository.GetStudentById(student.Id); 
            if (existinStudent == null) 
            { 
                return NotFound($"Student with id {student.Id} not found"); 
            } 
            bool status = Repository.UpdateStudent(student); 
            if (status) 
            { 
                return Ok(); 
            } 
            return BadRequest("Something went wrong"); 
        } 
        [HttpDelete("{id}")] 
        public ActionResult DeleteStudent([FromRoute] int id) { 
            Student existingStudent = Repository.GetStudentById(id); 
            if (existingStudent == null) 
            { 
                return NotFound($"Student with id {id} not found"); 
            } 
            bool status = Repository.DeleteStudent(id); 
            if (status) 
            { 
                return NoContent(); 
            } 
            return BadRequest($"Unable to delete student with id {id}");         
        } 
    } 
} 

*/
