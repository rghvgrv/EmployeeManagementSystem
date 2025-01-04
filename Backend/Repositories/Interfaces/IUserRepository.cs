using EmployeeManagementSystem.Models.DTOs;
using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUserById(int id);
        UserDTO GetUserByName(string name);
        void AddUser(UserCreateDTO user);
        void UpdateUser(UserDTO user);
    }
}
