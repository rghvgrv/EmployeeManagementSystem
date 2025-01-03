using EmployeeManagementSystem.DB;
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
    }
}
