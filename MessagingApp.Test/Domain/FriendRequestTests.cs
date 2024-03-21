using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Test.Domain;

public class FriendRequestTests
{
    [Fact]
    public void CreateFriendRequest_With_ValidUsers()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user2);
        Assert.True(friendRequestResult.IsOk);

        var friendRequest = friendRequestResult.Value;
        Assert.Equal(user1, friendRequest.SendingUser);
        Assert.Equal(user2, friendRequest.ReceivingUser);
    }
    
    [Fact]
    public void CreateFriendRequest_With_SameUser()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");

        var user1 = user1Result.Value;

        var friendRequestResult = FriendRequest.CreateFriendRequest(user1, user1);
        Assert.False(friendRequestResult.IsOk);
    }
}