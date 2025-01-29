using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly EmployeeDBContext _employeeDBContext;
        private readonly IConfiguration _configuration;
        private readonly IJWTServices _jwtService;

        public AuthController(EmployeeDBContext employeeDBContext, IConfiguration configuration, IJWTServices jwtServices)
        {
            _employeeDBContext = employeeDBContext;
            _configuration = configuration;
            _jwtService = jwtServices;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthUser user)
        {
            var loginuser = await _employeeDBContext.User.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (loginuser == null)
            {
                return Unauthorized("User name is Invalid");
            }

            user.Password = HashingServices.HashPassword(user.Password);

            if (loginuser.PasswordHash != user.Password)
            {
                return Unauthorized("Password is Invalid");
            }

            var activeSession = await _employeeDBContext.Authentication.FirstOrDefaultAsync(e => e.LoginUserId == loginuser.Id && e.IsActive == true);
            if (activeSession != null)
            {
                // Deactivate the current active session
                activeSession.IsActive = false;
                activeSession.LogoutTime = DateTime.Now;
                await _employeeDBContext.SaveChangesAsync();
            }

            // Create authentication record first
            var loginActivity = new Authentication
            {
                LoginUserId = loginuser.Id,
                LoginTime = DateTime.Now,
                LogoutTime = DateTime.Now.AddMinutes(15),
                IsActive = true
            };

            await _employeeDBContext.Authentication.AddAsync(loginActivity);
            await _employeeDBContext.SaveChangesAsync();

            // Generate JWT Token using authentication ID and username
            var token = _jwtService.GenerateToken(loginActivity.Id);

            // Update the authentication record with the generated token
            loginActivity.AuthKey = token;
            await _employeeDBContext.SaveChangesAsync();

            // Return the token along with authentication ID
            return Ok(new
            {
                Token = token,
                message = "Login Successful",
                username = user.Username,
                userid = loginuser.Id,
                authId = loginActivity.Id
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest request)
        {
            var empId = request.EmployeeUserId;

            if (empId == null)
            {
                return Unauthorized("User not found.");
            }

            // Fetch the user from the database
            var user = _employeeDBContext.Authentication.FirstOrDefault(e => e.LoginUserId == empId && e.IsActive == true);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Set IsActive to false
            user.IsActive = false;
            user.LogoutTime = DateTime.Now;

            // Save changes to the database
            _employeeDBContext.SaveChanges();

            return Ok("Logout Successful");
        }

        [HttpGet("validate-session/{userId}")]
        public IActionResult ValidateSession(int userId)
        {
            try
            {
                // Extract the token from the Authorization header
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Authorization token is missing.");
                }

                var authIdClaim = _jwtService.GetUserIdFromToken(token);

                if (authIdClaim == null)
                {
                    return Unauthorized("Invalid token.");
                }

                var authId = int.Parse(authIdClaim);

                var session = _employeeDBContext.Authentication
                    .FirstOrDefault(e => e.Id == authId && e.IsActive == true);

                // If session is valid, return success response
                return Ok("Session valid.");
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 error
                return Unauthorized("Token Expired.");
            }
        }

        // Create a model to bind the request body
        public class LogoutRequest
        {
            public int EmployeeUserId { get; set; }
        }

    }
}
