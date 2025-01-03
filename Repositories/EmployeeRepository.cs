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

    public Employee GetEmployeeById(int id)
    {
        return _empDBContext.Employee.Find(id);
    }

    public void AddEmployee(Employee employee)
    {
        _empDBContext.Employee.Add(employee);
        _empDBContext.SaveChanges();
    }

    public void UpdateEmployee(Employee employee)
    {
        _empDBContext.Employee.Update(employee);
        _empDBContext.SaveChanges();
    }

    public void DeleteEmployee(int id)
    {
        var employee = _empDBContext.Employee.Find(id);
        _empDBContext.Employee.Remove(employee);
        _empDBContext.SaveChanges();
    }
}
