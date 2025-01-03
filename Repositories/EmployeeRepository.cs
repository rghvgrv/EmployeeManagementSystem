using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDBContext _empDBContext;

    public EmployeeRepository(EmployeeDBContext context)
    {
        _empDBContext = context;
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return _empDBContext.Employee.ToList();
    }
}
