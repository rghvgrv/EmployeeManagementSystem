using EmployeeManagementSystem.DB;
using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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

                    if (string.IsNullOrEmpty(token))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Authorization token is missing.");
                        return;
                    }

                    var authIdClaim = _jwtService.GetUserIdFromToken(token);

                    if (authIdClaim == null || !int.TryParse(authIdClaim, out int authId))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token.");
                        return;
                    }

                    var session = _employeeDBContext.Authentication.FirstOrDefault(e => e.Id == authId && e.IsActive == true);

                    if (session == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Session invalid or expired.");
                        return;
                    }

                    // Validate the token
                    if (!_jwtService.ValidateToken(token))
                    {
                        // Token expired but session is still active -> Issue a new token
                        var newToken = _jwtService.GenerateToken(authId);

                        context.Response.Headers.Add("New-Token", newToken);
                    }
                }
            }

            await _next(context);
        }
    }
}
