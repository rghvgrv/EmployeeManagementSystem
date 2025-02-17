﻿using EmployeeManagementSystem.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Services
{
    public class JWTServices : IJWTServices
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInMinutes;

        public JWTServices(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expiryInMinutes = int.Parse(configuration["Jwt:ExpiryInMinutes"]);
        }

        public string GenerateToken(int id)
        {
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var createToken = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = key
            };

            var output = new JwtSecurityTokenHandler().ValidateToken(token, createToken, out var validatedToken);

            if(output.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(token);
            var userId = securityToken.Claims.First(claim => claim.Type == "id").Value;
            return userId;
        }
    }
}
