using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class MessageTests
{
    [Fact]
    public void CreateMessage_With_ValidContent()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        var message = Message.CreateMessage(user1, conversation, "Valid Content");
        
        Assert.True(message.IsOk);
    }
    
    [Fact]
    public void CreateMessage_With_EmptyContent()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");

        var user1 = user1Result.Value;
        var user2 = user2Result.Value;

        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        var message = Message.CreateMessage(user1, conversation, "");
        
        Assert.False(message.IsOk);
    }
}