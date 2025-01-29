namespace EmployeeManagementSystem.Services.Interfaces
{
    public interface IJWTServices
    {
        string GenerateToken(int id);
        bool ValidateToken(string token);

        string GetUserIdFromToken(string token);
    }
}
