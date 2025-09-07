using Application.Dtos.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.IntegrationTests.Common
{
    public static class AuthenticationHelper
    {
        public static string GenerateToken(UserDto user, IConfiguration configuration)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user?.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user?.FullName ?? string.Empty),
                new Claim("avatarUrl", user?.AvatarUrl ?? string.Empty),
                new Claim("isMc", user.IsMc?.ToString().ToLowerInvariant() ?? "false"),
                new Claim("gender", user.Gender?.ToString() ?? string.Empty),
                new Claim("fullName", user?.FullName ?? string.Empty),
                new Claim("nickName", user?.NickName ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
