using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.Equals("/api/auth/login", StringComparison.OrdinalIgnoreCase))
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var _employeeDBContext = scope.ServiceProvider.GetRequiredService<EmployeeDBContext>();
                    var _jwtService = scope.ServiceProvider.GetRequiredService<IJWTServices>();

                    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                    if (string.IsNullOrEmpty(token) || !_jwtService.ValidateToken(token))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Authorization token is missing or invalid.");
                        return;
                    }

                    var userIdClaim = _jwtService.GetUserIdFromToken(token);
                    if (userIdClaim == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token.");
                        return;
                    }

                    var userId = _employeeDBContext.User.FirstOrDefault(u => u.Username == userIdClaim).Id;
                    if(userId == 0)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("User not found.");
                        return;
                    }

                    var session = _employeeDBContext.Authentication
                        .FirstOrDefault(e => e.LoginUserId == userId && e.IsActive == true && e.AuthKey == token);

                    if (session == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Session invalid or expired.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
