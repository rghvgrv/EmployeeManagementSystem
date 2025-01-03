namespace EmployeeManagementSystem.Models.DTOs
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }  = null!;
        public int EmpId { get; set; }
    }
}
