using MessagingApp.Application.Features.GetUser;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetUserTests
{
    [Fact]
    public async Task GetUser_With_ValidUser()
    {
        // Setup users
        var friend1 = DomainObjectFactory.CreateUser(username: "TestFriend1", id: Guid.NewGuid(), bio: "Test Bio");
        var user = DomainObjectFactory.CreateUser(
            username: "TestUser1",
            id: Guid.NewGuid(),
            bio: "Test Bio",
            friends: new List<User> { friend1 });
        
        Helpers.SetProperty(friend1, nameof(friend1.Friends), new List<User> { friend1 });

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new GetUserHandler(applicationContext);

        // Add users to in-memory database
        await applicationContext.Users.AddRangeAsync(user, friend1);
        await applicationContext.SaveChangesAsync();
        applicationContext.ChangeTracker.Clear();
        
        // Run query
        var query = new GetUserQuery { UserId = user.Id };
        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);
        
        var getUserResponse = handlerResult.Value;
        Assert.Equal(user.Id, getUserResponse.UserId);
        Assert.Equal(user.Username, getUserResponse.Username);
        Assert.Equal(user.Bio, getUserResponse.Bio);

        var containsCorrectFriend = getUserResponse.Friends
            .Any(x => x.Id == friend1.Id && x.Username == friend1.Username);
        Assert.Single(getUserResponse.Friends);
        Assert.True(containsCorrectFriend);
    }
}