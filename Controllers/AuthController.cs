using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            if(loginuser == null)
            {
                return Unauthorized("User name is Invalid");
            }

            if(loginuser.PasswordHash != user.Password)
            {
                return Unauthorized("Password is Invalid");
            }

            //Generate JWT Token
            var token = _jwtService.GenerateToken(user.Username);

            var loginActivity = new Authentication
            {
                LoginUserId = loginuser.Id,
                LoginTime = DateTime.Now
            };

            await _employeeDBContext.Authentication.AddAsync(loginActivity);
            await _employeeDBContext.SaveChangesAsync();

            // Return the token
            return Ok(new { Token = token });
        }

        [HttpPost("logogut")]
        public IActionResult Logout()
        {
            return Ok("Logout Successful");
        }


    }
}
