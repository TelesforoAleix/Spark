using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backendSpark.Model.Repositories;
using backendSpark.Model.Entities;

namespace backendSpark.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class LoginController : ControllerBase 
    { 
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous] 
        [HttpPost] 
        public ActionResult Login([FromBody] Login credentials) 
        { 
            // Look up the user in the database using the repository
            var user = _userRepository.GetUser(credentials.Username, credentials.Password);
            
            if (user != null) 
            { 
                // Generate authentication token
                var text = $"{credentials.Username}:{credentials.Password}"; 
                var bytes = System.Text.Encoding.UTF8.GetBytes(text); 
                var encodedCredentials = Convert.ToBase64String(bytes); 
                var headerValue = $"Basic {encodedCredentials}"; 
                
                return Ok(new { 
                    headerValue = headerValue,
                    userId = user.Id,
                    username = user.Username
                }); 
            } 
            else 
            { 
                return Unauthorized(); 
            } 
        } 
    } 
}