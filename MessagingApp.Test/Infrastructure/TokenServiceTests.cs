using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Models;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Infrastructure.Settings;
using MessagingApp.Test.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace MessagingApp.Test.Infrastructure;

public class TokenServiceTests
{
    [Fact]
    public async Task RotateTokens_With_ValidParameters()
    {
        // Create dependencies
        var jwtSettings = new JwtSettings
        {
            Issuer = "http://localhost",
            Audience = "http://localhost",
            AccessTokenLife = 60,
            RefreshTokenLife = 1440,
            Key = "vuuK2b7zQbjbvy96EfL8Hm5E2Op8DZCNJwZLg9mDbPlKCAvdCsR7eBoxvETsySYZpeaBucw8PGJniE9yd3/4tEOq91BbSY6KVga1J+BsDu+jt3EcaPQWvZAHw82hXVVCQMJXIgrKyomk63Ch2qEw0LdSPVm9uY/tlZWbOQryfmBSs9/5pE28d+CfWph6Mi63OIV8rD3OcXLAgQjisJ+TfolCc9IOa5UNCz3TmQ=="
        };
        var mockOptions = Substitute.For<IOptions<JwtSettings>>();
        mockOptions.Value.Returns(jwtSettings);
        
        ITokenContext tokenContext = new ApplicationContext(DatabaseSetup.Options);
        
        var testUserResult = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var testUser = testUserResult.Value;
        
        // Rotate tokens
        var tokenService = new TokenService(tokenContext, mockOptions);
        var tokenSetResult = await tokenService.RotateTokens(testUser);
        Assert.True(tokenSetResult.IsOk);
    }

    [Fact]
    public void GenerateAccessToken_With_ValidUser()
    {
        // Create dependencies
        var jwtSettings = new JwtSettings
        {
            Issuer = "http://localhost",
            Audience = "http://localhost",
            AccessTokenLife = 60,
            RefreshTokenLife = 1440,
            Key = "vuuK2b7zQbjbvy96EfL8Hm5E2Op8DZCNJwZLg9mDbPlKCAvdCsR7eBoxvETsySYZpeaBucw8PGJniE9yd3/4tEOq91BbSY6KVga1J+BsDu+jt3EcaPQWvZAHw82hXVVCQMJXIgrKyomk63Ch2qEw0LdSPVm9uY/tlZWbOQryfmBSs9/5pE28d+CfWph6Mi63OIV8rD3OcXLAgQjisJ+TfolCc9IOa5UNCz3TmQ=="
        };
        var mockOptions = Substitute.For<IOptions<JwtSettings>>();
        var mockContext = Substitute.For<ITokenContext>(); // Can mock context because GenerateAccessToken does not use it

        mockOptions.Value.Returns(jwtSettings);
        
        var testUserResult = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var testUser = testUserResult.Value;
        
        var tokenService = new TokenService(mockContext, mockOptions);
        var methodInfo = Helpers.GetPrivateMethodInfo(tokenService, "GenerateAccessToken");

        var accessToken = (string?)methodInfo.Invoke(tokenService, [ testUser ]) ?? throw new Exception("GenerateAccessToken returned null");
        
        // Assertions to ensure the JWT is valid
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        // Decode the token to inspect the payload without validating the signature
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);
        Assert.Equal(jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Contains(jwtSettings.Audience, jwtToken.Audiences);
        
        // Now validate the signature
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Handle clock skew by setting it to zero
        };

        tokenHandler.ValidateToken(accessToken, validationParameters, out var validatedToken);
        var validJwtToken = validatedToken as JwtSecurityToken ?? throw new Exception("JwtSecurityToken was null");


        var hasClaims =
            validJwtToken.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == testUser.Username);
        
        Assert.True(validJwtToken.ValidTo > DateTime.UtcNow); // Ensure token has not expired
        Assert.True(hasClaims); // Example of checking for a specific claim
    }
    
    [Fact]
    public void GenerateRefreshToken_With_ValidUser()
    {
        // Create dependencies
        var jwtSettings = new JwtSettings
        {
            Issuer = "http://localhost",
            Audience = "http://localhost",
            AccessTokenLife = 60,
            RefreshTokenLife = 1440,
            RefreshTokenLength = 64,
            Key = "vuuK2b7zQbjbvy96EfL8Hm5E2Op8DZCNJwZLg9mDbPlKCAvdCsR7eBoxvETsySYZpeaBucw8PGJniE9yd3/4tEOq91BbSY6KVga1J+BsDu+jt3EcaPQWvZAHw82hXVVCQMJXIgrKyomk63Ch2qEw0LdSPVm9uY/tlZWbOQryfmBSs9/5pE28d+CfWph6Mi63OIV8rD3OcXLAgQjisJ+TfolCc9IOa5UNCz3TmQ=="
        };
        var mockOptions = Substitute.For<IOptions<JwtSettings>>();
        var mockContext = Substitute.For<ITokenContext>(); // Can mock context because GenerateRefreshToken does not use it

        mockOptions.Value.Returns(jwtSettings);
        
        var testUserResult = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var testUser = testUserResult.Value;
        
        var tokenService = new TokenService(mockContext, mockOptions);
        var methodInfo = Helpers.GetPrivateMethodInfo(tokenService, "GenerateRefreshToken");

        var refreshToken = (RefreshToken?)methodInfo.Invoke(tokenService, [ testUser ]) ?? throw new Exception("GenerateRefreshToken returned null");
        Assert.NotNull(refreshToken.Token);
        
        var tokenBytes = Convert.FromBase64String(refreshToken.Token);
        
        Assert.Equal(testUser, refreshToken.Owner); // Check owner is equal
        Assert.True(refreshToken.ExpiryDate > DateTime.UtcNow); // Ensure token has not expired
        Assert.Equal(jwtSettings.RefreshTokenLength, tokenBytes.Length); // Ensure refresh token generated is the correct number of bytes
    }
}