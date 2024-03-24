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
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Username!)
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
        
        existingTokens.ForEach(x => x.Active = false);

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
    
    public Guid ExtractUserIdFromAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Value.Key);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _settings.Value.Issuer,
            ValidAudience = _settings.Value.Audience,
            ClockSkew = TimeSpan.Zero // Immediate expiration
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null) throw new InvalidOperationException("User ID was not found in JWT");
        
        return Guid.Parse(userIdClaim.Value);
    }
}