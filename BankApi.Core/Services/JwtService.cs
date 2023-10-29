using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankApp.Core.Services;
public class JwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateJwtToken()
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expireTime = DateTime.Now.AddHours(1);
        JwtSecurityToken token = new("localhost",
            "localhost",
            expires: expireTime,
            signingCredentials: credentials);

        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.WriteToken(token);
    }
}
