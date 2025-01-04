using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; private set; }
        public string Username { get; set; }  = null!;
        public int EmpId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Password { get; set; }

        public UserDTO(int id, string username, int empId)
        {
            Id = id;
            Username = username;
            EmpId = empId;
        }
    }

    public class UserCreateDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int EmpId { get; set; }
    }
}
