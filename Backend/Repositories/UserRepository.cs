using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDBContext _empDBContext;

        public UserRepository(EmployeeDBContext context)
        {
            _empDBContext = context;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _empDBContext.User.Select(u => new UserDTO(u.Id, u.Username, u.EmpId));
            return users;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _empDBContext.User
                           .Where(u => u.Id == id)
                           .Select(u => new UserDTO(u.Id,u.Username,u.EmpId))
                           .FirstOrDefault();
            return user ?? throw new KeyNotFoundException("Id is not correct");
        }

        public UserDTO GetUserByName(string name) 
        {
            var user = _empDBContext.User
                           .Where(u => u.Username == name)
                           .Select(u => new UserDTO(u.Id, u.Username, u.EmpId))
                           .FirstOrDefault();
            return user ?? throw new KeyNotFoundException("User not found.");
        }

        public void AddUser(UserCreateDTO user)
        {
            user.Password = HashingServices.HashPassword(user.Password);

            User userEntity = new User()
            {
                Username = user.Username,
                PasswordHash = user.Password,
                EmpId = user.EmpId
            };
            _empDBContext.User.Add(userEntity);
            _empDBContext.SaveChanges();
        }

        public void UpdateUser(UserDTO user)
        {
            user.Password = HashingServices.HashPassword(user.Password);

            User userEntity = new User()
            {
                Id = user.Id,
                Username = user.Username,
                PasswordHash = user.Password,
                EmpId = user.EmpId
            };
            _empDBContext.User.Update(userEntity);
            _empDBContext.SaveChanges();
        }

    }
}
