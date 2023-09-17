using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Entities;
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
    public string GenerateToken(User user)
    {
        string keyString = _config["Jwt:Key"] 
                           ?? throw new MissingConfigException("No JWT secret key has been configured");
        SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        SigningCredentials credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

        Claim[] claims = {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Username)
        };

        JwtSecurityToken token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}