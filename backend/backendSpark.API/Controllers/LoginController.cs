using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backendSpark.Model.Repositories;
using backendSpark.Model.Entities;

namespace backendSpark.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public LoginController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User loginData)
        {
            var user = _userRepo.GetUser(loginData.Username, loginData.Password);
            if (user != null)
            {
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}