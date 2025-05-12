using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backendSpark.Model.Repositories;
using backendSpark.Model.Entities;

namespace backendSpark.API.Controllers 
{   
    // This controller handles user login operations.
    // It uses the UserRepository to interact with the data layer.
    [Route("api/[controller]")] 
    [ApiController] 
    public class LoginController : ControllerBase 
    { 
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST api/login
        // This method handles user login requests.
        // It takes a Login object as input and returns an authentication token (Basic Authentication) if successful.
        [AllowAnonymous]  // It can be accessed without authentication
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