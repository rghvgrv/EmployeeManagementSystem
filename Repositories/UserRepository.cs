using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;

namespace EmployeeManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDBContext _empDBContext;

        public UserRepository(EmployeeDBContext context)
        {
            _empDBContext = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _empDBContext.User.ToList();
        }
    }
}
