using MessagingApp.Application.Features.RespondToFriendRequest;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class RespondToFriendRequestTests
{
    [Fact]
    public async Task AcceptFriendRequest_With_ValidParameters()
    {
        // Create users
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());

        // Create friend request
        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var command = new RespondToFriendRequestCommand
        {
            FriendRequestId = friendRequest.Id,
            RespondingUserId = user2.Id,
            Response = FriendRequestStatus.Accepted
        };
        
        var handler = new RespondToFriendRequestHandler(applicationContext);
        var handlerResult = await handler.Handle(command);
        Assert.True(handlerResult.IsOk);

        Assert.Contains(user2, user1.Friends);
        Assert.Contains(user1, user2.Friends);
        Assert.Equal(FriendRequestStatus.Accepted, friendRequest.Status);
    }
    
    [Fact]
    public async Task RejectFriendRequest_With_ValidParameters()
    {
        // Create users
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());

        // Create friend request
        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var command = new RespondToFriendRequestCommand
        {
            FriendRequestId = friendRequest.Id,
            RespondingUserId = user2.Id,
            Response = FriendRequestStatus.Rejected
        };
        
        var handler = new RespondToFriendRequestHandler(applicationContext);
        var handlerResult = await handler.Handle(command);
        Assert.True(handlerResult.IsOk);

        Assert.DoesNotContain(user2, user1.Friends);
        Assert.DoesNotContain(user1, user2.Friends);
        Assert.Equal(FriendRequestStatus.Rejected, friendRequest.Status);
    }
    
    
    [Fact]
    public async Task AcceptFriendFriendRequest_After_AcceptingFriendRequest()
    {
        // Create users
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());

        // Create friend request
        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        // Pre-accept friend request and add friends
        Helpers.SetProperty(friendRequest, nameof(friendRequest.Status), FriendRequestStatus.Accepted);
        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var command = new RespondToFriendRequestCommand
        {
            FriendRequestId = friendRequest.Id,
            RespondingUserId = user2.Id,
            Response = FriendRequestStatus.Rejected
        };
        
        var handler = new RespondToFriendRequestHandler(applicationContext);
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);

        Assert.Contains(user2, user1.Friends);
        Assert.Contains(user1, user2.Friends);
        Assert.Equal(FriendRequestStatus.Accepted, friendRequest.Status);
    }
    
    [Fact]
    public async Task RespondToFriendRequest_For_DifferentUsers()
    {
        // Create users
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");
        var user3 = DomainObjectFactory.CreateUser(username: "TestUser3");
        
        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());
        Helpers.SetId(user3, Guid.NewGuid());
        
        // Create friend request
        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.Users.AddRangeAsync(user1, user2, user3);
        await applicationContext.SaveChangesAsync();

        var command = new RespondToFriendRequestCommand
        {
            FriendRequestId = friendRequest.Id,
            RespondingUserId = user3.Id,
            Response = FriendRequestStatus.Rejected
        };
        
        var handler = new RespondToFriendRequestHandler(applicationContext);
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);

        Assert.Empty(user1.Friends);
        Assert.Empty(user2.Friends);
        Assert.Empty(user3.Friends);
        Assert.Equal(FriendRequestStatus.Pending, friendRequest.Status);
    }
}