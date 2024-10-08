﻿using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Services;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class UserTests
{
    [Fact]
    public void CreateUser_With_ValidParameters()
    {
        var securityService = new SecurityService();
        
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "This is a biography!", securityService.HashPassword);
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!", null, securityService.HashPassword);
        
        // Assert User 1 was created successfully
        Assert.True(user1Result.IsOk);
        var user1 = user1Result.Value;
        
        Assert.Equal("TestUser1", user1.Username);
        Assert.Equal("This is a biography!", user1.Bio);        
        Assert.True(BCrypt.Net.BCrypt.Verify("TestPassword1!", user1.HashedPassword));
        Assert.True(user1.Active);
        
        // Only check that user was created and bio is null, everything else the same as test user 1
        Assert.True(user2Result.IsOk);
        var user2 = user2Result.Value;
        Assert.Null(user2.Bio);
    }

    [Fact]
    public void CreateUser_With_InvalidUsername()
    {
        var securityService = new SecurityService();
        var user1Result = User.CreateNewUser("Test1", "TestPassword1!", "Bio", securityService.HashPassword); // Too short
        var user2Result = User.CreateNewUser("TestUser2!", "TestPassword2!", "Bio", securityService.HashPassword); // Contains symbols
        
        Assert.False(user1Result.IsOk);
        Assert.False(user2Result.IsOk);
    }

    [Fact]
    public void CreateUser_With_InvalidPassword()
    {
        var securityService = new SecurityService();
        var user1Result = User.CreateNewUser("TestUser1", "Test1!", "Bio", securityService.HashPassword); // Too short
        var user2Result = User.CreateNewUser("TestUser2", "testpassword1!", "Bio", securityService.HashPassword); // No upper
        var user3Result = User.CreateNewUser("TestUser3", "TestPassword!", "Bio", securityService.HashPassword); // No digit
        var user4Result = User.CreateNewUser("TestUser4", "TestPassword1", "Bio", securityService.HashPassword); // No symbol
        
        Assert.False(user1Result.IsOk);
        Assert.False(user2Result.IsOk);
        Assert.False(user3Result.IsOk);
        Assert.False(user4Result.IsOk);
    }

    [Fact]
    public void RemoveFriend_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        // Make users friends
        Helpers.SetProperty(user1, nameof(User.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(User.Friends), new List<User> { user1 });

        var removeResult = user2.RemoveFriend(user1);
        Assert.True(removeResult.IsOk);
        
        Assert.DoesNotContain(user1, user2.Friends);
        Assert.DoesNotContain(user2, user1.Friends);
    }
    
    [Fact]
    public void RemoveFriend_With_NonFriendUser()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");
        var user3 = DomainObjectFactory.CreateUser(username: "TestUser3");
        
        // Make users friends
        Helpers.SetProperty(user1, nameof(User.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(User.Friends), new List<User> { user1 });

        var removeResult = user3.RemoveFriend(user1);
        Assert.False(removeResult.IsOk);
        
        Assert.Contains(user1, user2.Friends);
        Assert.Contains(user2, user1.Friends);
    }
    
    [Fact]
    public void SendFriendRequest_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        var friendRequestResult = user1.SendFriendRequest(user2);
        Assert.True(friendRequestResult.IsOk);

        var friendRequest = friendRequestResult.Value;

        Assert.Contains(friendRequest, user1.SentFriendRequests);
        Assert.Contains(friendRequest, user2.ReceivedFriendRequests);
        Assert.Equal(FriendRequestStatus.Pending, friendRequest.Status);
    }

    [Fact]
    public void SendFriendRequest_To_Self()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var friendRequestResult = user1.SendFriendRequest(user1);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void SendFriendRequest_To_Friend()
    {
        // Create users
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

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
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

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
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Rejected);
        
        Assert.Equal(FriendRequestStatus.Rejected, friendRequest.Status);
        Assert.DoesNotContain(user2, user1.Friends);
        Assert.DoesNotContain(user1, user2.Friends);
    }

    [Fact]
    public void Respond_To_FriendRequest_With_SendingUser()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        friendRequestResult = user1.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void Respond_To_FriendRequest_With_InvalidStatus()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        friendRequestResult = user1.RespondToFriendRequest(friendRequest, FriendRequestStatus.Pending);
        Assert.False(friendRequestResult.IsOk);
    }

    [Fact]
    public void Respond_To_NonPending_FriendRequest()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2");

        var friendRequestResult = user1.SendFriendRequest(user2);
        var friendRequest = friendRequestResult.Value;

        user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        
        // Respond a second time
        friendRequestResult = user2.RespondToFriendRequest(friendRequest, FriendRequestStatus.Accepted);
        Assert.False(friendRequestResult.IsOk);
    }
}