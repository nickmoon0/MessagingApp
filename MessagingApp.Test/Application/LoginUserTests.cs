using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.LoginUser;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Infrastructure.Settings;
using MessagingApp.Test.Common;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace MessagingApp.Test.Application;

public class LoginUserTests
{
    [Fact]
    public async Task LoginUser_With_MatchingCredentials()
    {
        var command = new LoginUserCommand
        {
            Username = "TestUser1",
            Password = "TestPassword1!"
        };
        
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
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = new TokenService(applicationContext, mockOptions);
        var securityService = new SecurityService();
        
        // Create and add a user with the username "TestUser1"
        var existingUser = DomainObjectFactory.CreateUser(
            username: command.Username,
            hashedPassword: BCrypt.Net.BCrypt.HashPassword(command.Password));
        
        applicationContext.Users.Add(existingUser);
        await applicationContext.SaveChangesAsync();

        var handler = new LoginUserHandler(applicationContext, tokenService, securityService);
        var result = await handler.Handle(command);
        
        Assert.True(result.IsOk);
        Assert.NotNull(result.Value.Tokens.NewAccessToken);
        Assert.NotNull(result.Value.Tokens.NewRefreshToken);
    }

    [Fact]
    public async Task LoginUser_With_InvalidCredentials()
    {
        var command = new LoginUserCommand
        {
            Username = "TestUser1",
            Password = "WrongPassword1!"
        };
        
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
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = new TokenService(applicationContext, mockOptions);
        var securityService = new SecurityService();
        
        // Create and add a user with the username "TestUser1"
        var existingUser = DomainObjectFactory.CreateUser(
            username: command.Username,
            hashedPassword: BCrypt.Net.BCrypt.HashPassword("TestPassword1!"));
        
        applicationContext.Users.Add(existingUser);
        await applicationContext.SaveChangesAsync();

        var handler = new LoginUserHandler(applicationContext, tokenService, securityService);
        var result = await handler.Handle(command);
        
        Assert.False(result.IsOk);
    }
}