using MessagingApp.Application.Features.SendFriendRequest;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Test.Application;

public class SendFriendRequestTests
{
    [Fact]
    public async Task SendFriendRequest_With_ValidParameters()
    {
        // Mock password hash as it doesnt apply to this test
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", null, x => x);
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", null, x => x);

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();
        
        var command = new SendFriendRequestCommand
        {
            SendingUserId = user1.Id,
            ReceivingUserId = user2.Id
        };
        
        var handler = new SendFriendRequestHandler(applicationContext);
        var result = await handler.Handle(command);
        
        Assert.True(result.IsOk);

        var friendRequest = await applicationContext.FriendRequests
            .Include(x => x.SendingUser)
            .Include(x => x.ReceivingUser)
            .SingleOrDefaultAsync(x => x.Id == result.Value.Id);
        
        Assert.NotNull(friendRequest);
        Assert.Equal(user1, friendRequest.SendingUser);
        Assert.Equal(user2, friendRequest.ReceivingUser);
        Assert.Contains(friendRequest, user1.SentFriendRequests);
        Assert.Contains(friendRequest, user2.ReceivedFriendRequests);
    }
    
    [Fact]
    public async Task SendFriendRequest_To_SameUser()
    {
        // Mock password hash as it doesnt apply to this test
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", null, x => x);

        var user1 = user1Result.Value;
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        await applicationContext.Users.AddAsync(user1);
        await applicationContext.SaveChangesAsync();
        
        var command = new SendFriendRequestCommand
        {
            SendingUserId = user1.Id,
            ReceivingUserId = user1.Id
        };
        
        var handler = new SendFriendRequestHandler(applicationContext);
        var result = await handler.Handle(command);
        
        Assert.False(result.IsOk);

        Assert.Empty(user1.SentFriendRequests);
        Assert.Empty(user1.ReceivedFriendRequests);
    }
}