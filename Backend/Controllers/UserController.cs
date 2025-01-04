using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("/GetAllUsers")]

        public IActionResult GetAll()
        {
            return Ok(_userRepository.GetAllUsers());
        }

        [HttpGet]
        [Route("/GetUserById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_userRepository.GetUserById(id));
        }

        [HttpGet]
        [Route("/GetUserByName/{name}")]
        public IActionResult GetByName(string name)
        {
            return Ok(_userRepository.GetUserByName(name));
        }


        [HttpPost]
        [Route("/AddUser")]
        public IActionResult Add([FromBody] UserCreateDTO user)
        {
            _userRepository.AddUser(user);
            return Ok(new { message = "User Added" });
        }

        [HttpPut]
        [Route("/UpdateUser")]
        public IActionResult Update([FromBody] UserDTO user)
        {
            _userRepository.UpdateUser(user);
            return Ok("User is Updated");
        }

    }
}
