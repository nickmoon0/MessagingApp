using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Models;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingApp.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly ITokenContext _tokenContext;
    private readonly IOptions<JwtSettings> _settings;
    
    public TokenService(ITokenContext tokenContext, IOptions<JwtSettings> settings)
    {
        _tokenContext = tokenContext;
        _settings = settings;
    }

    private string GenerateAccessToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username!)
        ];
        
        var token = new JwtSecurityToken(
            _settings.Value.Issuer,
            _settings.Value.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.Value.AccessTokenLife),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(User user)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(_settings.Value.RefreshTokenLength));
        var refreshToken = RefreshToken.CreateRefreshToken(
            user,
            token,
            DateTime.UtcNow.AddDays(_settings.Value.RefreshTokenLife)
        );

        return refreshToken;
    }

    public async Task<Result<TokenSet>> RotateTokens(User user)
    {
        // Get all active tokens that belong to current user and de-active them
        var existingTokens = await _tokenContext.RefreshTokens
            .Where(x => x.Active == true && x.Owner == user)
            .ToListAsync();
        
        existingTokens.ForEach(x => x.InactivateToken());

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user);

        await _tokenContext.RefreshTokens.AddAsync(refreshToken);
        await _tokenContext.SaveChangesAsync();

        return new TokenSet
        {
            NewAccessToken = accessToken,
            NewRefreshToken = refreshToken
        };
    }
}