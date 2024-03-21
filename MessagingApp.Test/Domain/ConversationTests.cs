using MessagingApp.Domain.Aggregates;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class ConversationTests
{
    [Fact]
    public void CreateDirectMessage_With_ValidUsers()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());

        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        Assert.True(conversationResult.IsOk);
    }

    [Fact]
    public void CreateDirectMessage_With_SameUser()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");

        var user1 = user1Result.Value;

        Helpers.SetId(user1, Guid.NewGuid());

        var conversationResult = Conversation.CreateDirectMessage(user1, user1);
        Assert.False(conversationResult.IsOk);
    }
}