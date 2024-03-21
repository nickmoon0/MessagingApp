using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class FriendRequestTests
{
    [Fact]
    public void SendFriendRequest_With_ValidParameters()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        Assert.True(friendRequestResult.IsOk);

        var friendRequest = friendRequestResult.Value;

        Assert.Contains(friendRequest, user1.FriendRequests);
        Assert.Contains(friendRequest, user2.FriendRequests);
        Assert.Equal(FriendRequestStatus.Pending, friendRequest.Status);
    }

    [Fact]
    public void SendFriendRequest_To_Self()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user1 = user1Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user1);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void SendFriendRequest_To_Friend()
    {
        // Create users
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        // Make users friends
        Helpers.SetProperty(user1, nameof(User.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(User.Friends), new List<User> { user1 });

        // Assert that a friend request cannot be sent
        var friendRequestResult = user1.SendFriendRequest(user2);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void AcceptFriendRequest_With_ValidParameters()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        
        Assert.Equal(FriendRequestStatus.Accepted, friendRequest.Status);
        Assert.Contains(user2, user1.Friends);
        Assert.Contains(user1, user2.Friends);
    }

    [Fact]
    public void RejectFriendRequest_With_ValidParameters()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Rejected);
        
        Assert.Equal(FriendRequestStatus.Rejected, friendRequest.Status);
        Assert.DoesNotContain(user2, user1.Friends);
        Assert.DoesNotContain(user1, user2.Friends);
    }

    [Fact]
    public void Respond_To_FriendRequest_With_InvalidUser()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        friendRequestResult = user1.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void Respond_To_FriendRequest_With_InvalidStatus()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        friendRequestResult = user1.RespondToFriendRequest(friendRequest, FriendRequestStatus.Pending);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void Respond_To_NonPending_FriendRequest()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "Bio");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", "Bio");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        
        // Respond a second time
        friendRequestResult = user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        Assert.False(friendRequestResult.IsOk);
    }
}