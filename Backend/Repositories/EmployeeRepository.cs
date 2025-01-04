using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using System.Linq;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDBContext _empDBContext;
    private readonly IUserRepository _userRepo;

    public EmployeeRepository(EmployeeDBContext context, IUserRepository userRepository)
    {
        _empDBContext = context;
        _userRepo = userRepository;
    }

    public IEnumerable<EmployeeDTO> GetAllEmployees()
    {
        var employees = _empDBContext.Employee
                    .Select(e => new EmployeeDTO(
                     e.EmpId,
                     e.FirstName,
                     e.LastName,
                     e.Role,
                     e.Department,
                     e.CreatedDate,
                     e.ModifiedDate,  
                     e.ModifiedBy,    
                     e.CreatedBy      
                     ))
                    .ToList();
        return employees;
    }

    public EmployeeDTO GetEmployeeById(int id)
    {
        var emp = _empDBContext.Employee
                         .Where(e => e.EmpId == id)
                         .Select(e => new EmployeeDTO(
                     e.EmpId,
                     e.FirstName,
                     e.LastName,
                     e.Role,
                     e.Department,
                     e.CreatedDate,
                     e.ModifiedDate,
                     e.ModifiedBy,
                     e.CreatedBy
                     ))
                         .FirstOrDefault();
        return emp ?? throw new KeyNotFoundException("Id is not correct");
    }

    public void AddEmployee(EmployeeCreateDTO employee)
    {
        Employee emp = new Employee()
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Role = employee.Role,
            Department = employee.Department,
            CreatedDate = employee.CreatedDate,
            ModifiedDate = DateTime.Now,
            ModifiedBy = employee.ModifiedBy,
            CreatedBy = employee.CreatedBy
        };
        _empDBContext.Employee.Add(emp);
        _empDBContext.SaveChanges();
    }

    public void UpdateEmployee(EmployeeCreateDTO employee)
    {
        Employee empEntity = new Employee()
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Role = employee.Role,
            Department = employee.Department,
            CreatedDate = employee.CreatedDate,
            ModifiedDate = DateTime.Now,
            ModifiedBy = employee.ModifiedBy,
            CreatedBy = employee.CreatedBy
        };
        _empDBContext.Employee.Update(empEntity);
        _empDBContext.SaveChanges();
    }

    public EmployeeDTO GetEmployeeByUserId(int userId)
    {
        var user = _userRepo.GetUserById(userId);

        var emp = _empDBContext.Employee
                        .Where(e => e.EmpId == user.EmpId)
                        .Select(e => new EmployeeDTO(
                    e.EmpId,
                    e.FirstName,
                    e.LastName,
                    e.Role,
                    e.Department,
                    e.CreatedDate,
                    e.ModifiedDate,
                    e.ModifiedBy,
                    e.CreatedBy
                    ))
                        .FirstOrDefault();
        return emp ?? throw new KeyNotFoundException("Id is not correct");
    }
}
