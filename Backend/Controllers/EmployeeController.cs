
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IJWTServices _jwtService;

        public EmployeeController(IEmployeeRepository employeeRepository,IJWTServices jWTServices)
        {
            this.employeeRepository = employeeRepository;
            this._jwtService = jWTServices;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAll()
        {
            return Ok(employeeRepository.GetAllEmployees());
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(employeeRepository.GetEmployeeById(id));
        }

        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult Add([FromBody] EmployeeCreateDTO employee)
        {
            employeeRepository.AddEmployee(employee);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        public IActionResult Update([FromBody] EmployeeCreateDTO employee)
        {
            employeeRepository.UpdateEmployee(employee);
            return Ok();
        }

        [HttpGet]
        [Route("GetEmployeeByUserId/{id}")]
        public IActionResult GetEmployeeUserId(int id)
        {
            return Ok(employeeRepository.GetEmployeeByUserId(id));
        }

    }
}
