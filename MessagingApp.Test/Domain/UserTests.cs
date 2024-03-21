using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Test.Domain;

public class UserTests
{
    [Fact]
    public void CreateUser_With_ValidParameters()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!", "This is a biography!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");
        
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
        var user1Result = User.CreateNewUser("Test1", "TestPassword1!", "Bio"); // Too short
        var user2Result = User.CreateNewUser("TestUser2!", "TestPassword2!", "Bio"); // Contains symbols
        
        Assert.False(user1Result.IsOk);
        Assert.False(user2Result.IsOk);
    }

    [Fact]
    public void CreateUser_With_InvalidPassword()
    {
        var user1Result = User.CreateNewUser("TestUser1", "Test1!", "Bio"); // Too short
        var user2Result = User.CreateNewUser("TestUser2", "testpassword1!", "Bio"); // No upper
        var user3Result = User.CreateNewUser("TestUser3", "TestPassword!", "Bio"); // No digit
        var user4Result = User.CreateNewUser("TestUser4", "TestPassword1", "Bio"); // No symbol
        
        Assert.False(user1Result.IsOk);
        Assert.False(user2Result.IsOk);
        Assert.False(user3Result.IsOk);
        Assert.False(user4Result.IsOk);
    }
}