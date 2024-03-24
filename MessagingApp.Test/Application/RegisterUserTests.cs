using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Application.Features.RegisterUser;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Infrastructure.Settings;
using MessagingApp.Test.Common;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace MessagingApp.Test.Application;

public class RegisterUserTests
{
    [Fact]
    public async Task RegisterUser_With_ValidParameters()
    {
        var request = new RegisterUserCommand
        {
            Username = "TestUser1",
            Password = "TestPassword1!",
            Bio = "This is a bio"
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

        var handler = new RegisterUserHandler(applicationContext, tokenService);

        var registerResult = await handler.Handle(request);
        Assert.True(registerResult.IsOk);

        var result = registerResult.Value;
        var user = await applicationContext.Users.FindAsync(result.Id);
        
        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(request.Username, user.Username);
        Assert.Equal(request.Bio, user.Bio);
        Assert.True(BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword));
    }

    [Fact]
    public async Task RegisterUser_With_InvalidUsername()
    {
        var request = new RegisterUserCommand
        {
            Username = "Test", // Too short
            Password = "TestPassword1!",
            Bio = "This is a bio"
        };
        
        // Mock service and context as handler should never reach the stage that its accessing either
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = Substitute.For<ITokenService>();
        
        var handler = new RegisterUserHandler(applicationContext, tokenService);
        
        var registerResult = await handler.Handle(request);
        Assert.False(registerResult.IsOk);
        
        Assert.True(true);
    }
    
    [Fact]
    public async Task RegisterUser_With_TakenUsername()
    {
        var request = new RegisterUserCommand
        {
            Username = "TestUser1",
            Password = "TestPassword1!",
            Bio = "This is a bio"
        };
        
        // Mock service and context as handler should never reach the stage that its accessing either
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = Substitute.For<ITokenService>();
        
        // Create and add a user with the username "TestUser1"
        var existingUser = DomainObjectFactory.CreateUser(username: "TestUser1");
        applicationContext.Users.Add(existingUser);
        await applicationContext.SaveChangesAsync();
        
        var handler = new RegisterUserHandler(applicationContext, tokenService);
        
        var registerResult = await handler.Handle(request);
        Assert.False(registerResult.IsOk);
    }
    
    [Fact]
    public async Task RegisterUser_With_InvalidPassword()
    {
        var request = new RegisterUserCommand
        {
            Username = "TestUser1",
            Password = "TestPassword", // No Number
            Bio = "This is a bio"
        };
        
        // Mock service and context as handler should never reach the stage that its accessing either
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var tokenService = Substitute.For<ITokenService>();
        
        var handler = new RegisterUserHandler(applicationContext, tokenService);
        
        var registerResult = await handler.Handle(request);
        Assert.False(registerResult.IsOk);
        
        Assert.True(true);
    }
}