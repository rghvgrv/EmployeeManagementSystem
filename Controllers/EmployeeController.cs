
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("/GetAllEmployees")]
        public IActionResult GetAll()
        {
            return Ok(employeeRepository.GetAllEmployees());
        }

        [HttpGet]
        [Route("/GetEmployeeById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(employeeRepository.GetEmployeeById(id));
        }

        [HttpPost]
        [Route("/AddEmployee")]
        public IActionResult Add([FromBody] EmployeeCreateDTO employee)
        {
            employeeRepository.AddEmployee(employee);
            return Ok();
        }

        [HttpPut]
        [Route("/UpdateEmployee")]
        public IActionResult Update([FromBody] EmployeeCreateDTO employee)
        {
            employeeRepository.UpdateEmployee(employee);
            return Ok();
        }

    }
}
