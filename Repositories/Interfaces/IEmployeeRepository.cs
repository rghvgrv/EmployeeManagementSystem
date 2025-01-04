using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeDTO> GetAllEmployees();
        EmployeeDTO GetEmployeeById(int id);
        void AddEmployee(EmployeeCreateDTO employee);
        void UpdateEmployee(EmployeeCreateDTO employee);

    }
}
