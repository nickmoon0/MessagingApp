using MessagingApp.Application.Features.GetFriendRequests;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetFriendRequestTests
{
    [Fact]
    public async Task GetFriendRequests_With_NoFilters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new GetFriendRequestsHandler(applicationContext);

        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.SaveChangesAsync();
        
        applicationContext.ChangeTracker.Clear();
        
        var query1 = new GetFriendRequestsQuery
        {
            UserId = user1.Id,
            GetSentRequests = true,
            GetReceivedRequests = true,
            Status = null // Gets all statuses
        };

        var query2 = new GetFriendRequestsQuery
        {
            UserId = user2.Id,
            GetSentRequests = true,
            GetReceivedRequests = true,
            Status = null
        };
        
        var handlerResult1 = await handler.Handle(query1);
        var handlerResult2 = await handler.Handle(query2);
        
        Assert.True(handlerResult1.IsOk);
        Assert.True(handlerResult2.IsOk);

        var response1 = handlerResult1.Value;
        var response2 = handlerResult2.Value;

        Assert.Single(response1.SentFriendRequests);
        Assert.Empty(response1.ReceivedFriendRequests);
        
        Assert.Single(response2.ReceivedFriendRequests);
        Assert.Empty(response2.SentFriendRequests);
    }
    
    [Fact]
    public async Task GetFriendRequests_With_AllFilters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        var friendRequest = friendRequestResult.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new GetFriendRequestsHandler(applicationContext);

        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.FriendRequests.AddAsync(friendRequest);
        await applicationContext.SaveChangesAsync();
        
        applicationContext.ChangeTracker.Clear();
        
        var query1 = new GetFriendRequestsQuery
        {
            UserId = user1.Id,
            GetSentRequests = false,
            GetReceivedRequests = true,
            Status = FriendRequestStatus.Pending
        };

        var query2 = new GetFriendRequestsQuery
        {
            UserId = user2.Id,
            GetSentRequests = true,
            GetReceivedRequests = false,
            Status = FriendRequestStatus.Pending
        };
        
        var handlerResult1 = await handler.Handle(query1);
        var handlerResult2 = await handler.Handle(query2);
        
        Assert.True(handlerResult1.IsOk);
        Assert.True(handlerResult2.IsOk);

        var response1 = handlerResult1.Value;
        var response2 = handlerResult2.Value;

        Assert.Empty(response1.SentFriendRequests);
        Assert.Empty(response1.ReceivedFriendRequests);
        
        Assert.Empty(response2.ReceivedFriendRequests);
        Assert.Empty(response2.SentFriendRequests);
    }
    
    [Fact]
    public async Task GetFriendRequests_With_NonExistentUser()
    {
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new GetFriendRequestsHandler(applicationContext);
        
        var query = new GetFriendRequestsQuery
        {
            UserId = Guid.NewGuid(),
            GetSentRequests = true,
            GetReceivedRequests = false,
            Status = FriendRequestStatus.Pending
        };
        
        var handlerResult = await handler.Handle(query);
        Assert.False(handlerResult.IsOk);
        
        
    }
}