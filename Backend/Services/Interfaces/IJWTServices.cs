namespace EmployeeManagementSystem.Services.Interfaces
{
    public interface IJWTServices
    {
        string GenerateToken(string username);
        bool ValidateToken(string token);

        string GetUserIdFromToken(string token);
    }
}
