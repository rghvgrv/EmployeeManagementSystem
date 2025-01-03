using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDBContext _empDBContext;

        public UserRepository(EmployeeDBContext context)
        {
            _empDBContext = context;
        }

        public IEnumerable<object> GetAllUsers()
        {
            return _empDBContext.User.Select(u => new { u.Id, u.Username,u.EmpId }).ToList();
        }

        public object GetUserById(int id)
        {
            var user =  _empDBContext.User.Where(u => u.Id == id).Select(u => new { u.Id, u.Username,u.EmpId }).FirstOrDefault();
            return user ?? throw new KeyNotFoundException("Id is not correct");
        }

        public object GetUserByName(string name) 
        {
            var user = _empDBContext.User
                           .Where(u => u.Username == name)
                           .Select(u => new { u.Id, u.Username, u.EmpId })
                           .FirstOrDefault();
            return user ?? throw new KeyNotFoundException("User not found.");
        }

        public void AddUser(User user)
        {
            user.PasswordHash = HashingServices.HashPassword(user.PasswordHash);
            _empDBContext.User.Add(user);
            _empDBContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _empDBContext.User.Update(user);
            _empDBContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _empDBContext.User.Find(id);
            _empDBContext.User.Remove(user);
            _empDBContext.SaveChanges();
        }

    }
}
