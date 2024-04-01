using MessagingApp.Application.Features.RenewTokens;
using MessagingApp.Application.Models;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Infrastructure.Settings;
using MessagingApp.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace MessagingApp.Test.Application;

public class RenewTokenTests
{
    [Fact]
    public async Task RenewToken_With_ValidRefreshToken()
    {
        // Setup dependencies
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
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
        var tokenContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = new TokenService(tokenContext, mockOptions);
        var handler = new RenewTokenHandler(tokenContext, tokenService);
        
        // Generate refresh token and insert into in-memory database
        var methodInfo = Helpers.GetPrivateMethodInfo(tokenService, "GenerateRefreshToken");
        var refreshToken = (RefreshToken?)methodInfo.Invoke(tokenService, [ user1 ]) ?? throw new Exception("GenerateRefreshToken returned null");
        await tokenContext.RefreshTokens.AddAsync(refreshToken);
        await tokenContext.SaveChangesAsync();
        
        // Run handler and make assertions
        var command = new RenewTokenCommand { RefreshToken = refreshToken.Token! };
        var handlerResult = await handler.Handle(command);
        Assert.True(handlerResult.IsOk);
        
        var response = handlerResult.Value;

        Assert.NotNull(response.Tokens.NewAccessToken);
        Assert.NotNull(response.Tokens.NewRefreshToken);
        
        Assert.NotEqual(Guid.Empty, response.Tokens.NewRefreshToken.Id);
        Assert.Equal(user1, response.Tokens.NewRefreshToken.Owner);
        
        // Ensure tokens are being deactivated
        var retrievedActiveTokens = await tokenContext.RefreshTokens
            .Where(x => x.Active)
            .ToListAsync();
        
        Assert.Single(retrievedActiveTokens);
    }

    [Fact]
    public async Task RenewToken_With_NonExistentToken()
    {
        // Setup dependencies
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
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
        var tokenContext = new ApplicationContext(DatabaseSetup.Options);
        
        var tokenService = new TokenService(tokenContext, mockOptions);
        var handler = new RenewTokenHandler(tokenContext, tokenService);
        
        
        // Run handler and make assertions
        var command = new RenewTokenCommand { RefreshToken = "" };
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);

        var retrievedTokens = await tokenContext.RefreshTokens.ToListAsync();
        Assert.Empty(retrievedTokens);
    }

    [Fact]
    public async Task RenewToken_With_InactiveToken()
    {
        // Setup dependencies
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
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
        var tokenContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = new TokenService(tokenContext, mockOptions);
        var handler = new RenewTokenHandler(tokenContext, tokenService);
        
        // Generate in-active refresh token and insert into in-memory database
        var methodInfo = Helpers.GetPrivateMethodInfo(tokenService, "GenerateRefreshToken");
        
        var refreshToken = (RefreshToken?)methodInfo.Invoke(tokenService, [ user1 ]) ?? throw new Exception("GenerateRefreshToken returned null");
        Helpers.SetProperty(refreshToken, nameof(refreshToken.Active), false);
        
        await tokenContext.RefreshTokens.AddAsync(refreshToken);
        await tokenContext.SaveChangesAsync();
        
        // Run handler and make assertions
        var command = new RenewTokenCommand { RefreshToken = refreshToken.Token! };
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        
        // Ensure tokens are being deactivated
        var retrievedActiveTokens = await tokenContext.RefreshTokens
            .Where(x => x.Active)
            .ToListAsync();
        
        Assert.Empty(retrievedActiveTokens);
    }

    [Fact]
    public async Task RenewToken_With_ExpiredToken()
    {
        // Setup dependencies
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
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
        var tokenContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = new TokenService(tokenContext, mockOptions);
        var handler = new RenewTokenHandler(tokenContext, tokenService);
        
        // Generate expired refresh token and insert into in-memory database
        var methodInfo = Helpers.GetPrivateMethodInfo(tokenService, "GenerateRefreshToken");
        
        var refreshToken = (RefreshToken?)methodInfo.Invoke(tokenService, [ user1 ]) ?? throw new Exception("GenerateRefreshToken returned null");
        Helpers.SetProperty(refreshToken, nameof(refreshToken.ExpiryDate), DateTime.UtcNow.AddDays(-1));
        
        await tokenContext.RefreshTokens.AddAsync(refreshToken);
        await tokenContext.SaveChangesAsync();
        
        // Run handler and make assertions
        var command = new RenewTokenCommand { RefreshToken = refreshToken.Token! };
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        
        // Ensure tokens are being deactivated
        var retrievedActiveTokens = await tokenContext.RefreshTokens
            .Where(x => x.Active)
            .ToListAsync();
        
        Assert.Empty(retrievedActiveTokens);
    }
}