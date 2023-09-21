using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MessagingApp.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    public string GenerateToken(UserDto user)
    {
        var key = _config["Jwt:Key"] ?? throw new MissingConfigException("No JWT key configured");
        var issuer = _config["Jwt:Issuer"] ?? throw new MissingConfigException("No JWT issuer configured");
        var audience = _config["Jwt:Audience"] ?? 
                       throw new MissingConfigException("No JWT audience configured");
        var tokenLife = int.Parse(_config["Jwt:TokenLife"] ?? 
                                  throw new MissingConfigException("No JWT token life configured"));

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

        Claim[] claims = {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Username!)
        };
        
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddMinutes(tokenLife),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}