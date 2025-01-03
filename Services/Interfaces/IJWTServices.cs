namespace EmployeeManagementSystem.Services.Interfaces
{
    public interface IJWTServices
    {
        string GenerateToken(string username);
    }
}
