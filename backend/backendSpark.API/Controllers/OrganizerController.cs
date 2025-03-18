using Microsoft.AspNetCore.Mvc;

namespace backendSpark.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizerController : ControllerBase
{
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
