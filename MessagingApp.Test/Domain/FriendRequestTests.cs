using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class FriendRequestTests
{
    [Fact]
    public void CreateFriendRequest_With_ValidUsers()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        Assert.True(friendRequestResult.IsOk);

        var friendRequest = friendRequestResult.Value;
        Assert.Equal(user1, friendRequest.SendingUser);
        Assert.Equal(user2, friendRequest.ReceivingUser);
    }
    
    [Fact]
    public void CreateFriendRequest_With_SameUser()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user1);
        Assert.False(friendRequestResult.IsOk);
    }
}