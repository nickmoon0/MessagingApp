using MessagingApp.Application.Features.GetFriends;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetFriendsTests
{
    [Fact]
    public async Task GetUsersFriends_With_Friends()
    {
        var user = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var friend1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
        Helpers.SetProperty(user, nameof(user.Friends), new List<User> { friend1 });
        Helpers.SetProperty(friend1, nameof(friend1.Friends), new List<User> { user });

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.Users.AddRangeAsync(user, friend1);
        await applicationContext.SaveChangesAsync();
        applicationContext.ChangeTracker.Clear();

        var handler = new GetFriendsHandler(applicationContext);
        var query = new GetFriendsQuery { UserId = user.Id };

        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;

        var friend1InResponse = response.Friends
            .Any(x => x.UserId == friend1.Id && x.Username == friend1.Username);
        
        Assert.Single(response.Friends); // Only one friend should be present
        Assert.True(friend1InResponse); // Check that correct friend is in response
    }
}