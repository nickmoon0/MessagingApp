using MessagingApp.Application.Features.GetUserByName;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetUserByNameTests
{
    [Fact]
    public async Task GetUserByName_With_ValidUsername_ReturnsUserDetails()
    {
        // Setup users
        var friend1 = DomainObjectFactory.CreateUser(username: "FriendUser", id: Guid.NewGuid(), bio: "Friend Bio");
        var targetUser = DomainObjectFactory.CreateUser(
            username: "TargetUser",
            id: Guid.NewGuid(),
            bio: "Target Bio",
            friends: new List<User> { friend1 });

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new GetUserByNameHandler(applicationContext);

        // Add users to in-memory database
        await applicationContext.Users.AddRangeAsync(targetUser, friend1);
        await applicationContext.SaveChangesAsync();
        applicationContext.ChangeTracker.Clear();

        // Run query by username
        var query = new GetUserByNameQuery { Username = "TargetUser" };
        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var getUserResponse = handlerResult.Value;
        Assert.Equal(targetUser.Id, getUserResponse.UserId);
        Assert.Equal(targetUser.Username, getUserResponse.Username);
        Assert.Equal(targetUser.Bio, getUserResponse.Bio);

        // Check if the friends are loaded correctly
        var containsCorrectFriend = getUserResponse.Friends
            .Any(x => x.Id == friend1.Id && x.Username == friend1.Username);
        Assert.Single(getUserResponse.Friends);
        Assert.True(containsCorrectFriend);
    }
}
