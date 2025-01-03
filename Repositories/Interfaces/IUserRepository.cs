using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<object> GetAllUsers();
        object GetUserById(int id);
        object GetUserByName(string name);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
