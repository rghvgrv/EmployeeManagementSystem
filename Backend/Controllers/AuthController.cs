﻿using EmployeeManagementSystem.DB;
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

            if(loginuser == null)
            {
                return Unauthorized("User name is Invalid");
            }

            user.Password = HashingServices.HashPassword(user.Password);

            if(loginuser.PasswordHash != user.Password)
            {
                return Unauthorized("Password is Invalid");
            }

            var status = await _employeeDBContext.Authentication.FirstOrDefaultAsync(e => e.LoginUserId == loginuser.Id && e.IsActive == true);
            {
                if (status != null)
                {
                    status.IsActive = false;
                    await _employeeDBContext.SaveChangesAsync();
                }
            }

            //Generate JWT Token
            var token = _jwtService.GenerateToken(user.Username);

            var loginActivity = new Authentication
            {
                LoginUserId = loginuser.Id,
                AuthKey = token,
                LoginTime = DateTime.Now,
                LogoutTime = DateTime.Now.AddMinutes(15),
                IsActive = true
            };

            await _employeeDBContext.Authentication.AddAsync(loginActivity);
            await _employeeDBContext.SaveChangesAsync();

            // Return the token
            return Ok(new { Token = token , message ="Login Successfull", username = user.Username , userid = loginuser.Id });
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

            // Save changes to the database
            _employeeDBContext.SaveChanges();

            return Ok("Logout Successful");
        }

        // Create a model to bind the request body
        public class LogoutRequest
        {
            public int EmployeeUserId { get; set; }
        }

    }
}
