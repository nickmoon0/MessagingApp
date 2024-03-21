using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
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

    [Fact]
    public void SendMessage_With_ValidParameters()
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
        
        var messageResult = conversation.SendMessage(user1, "Hello, World!");
        Assert.True(messageResult.IsOk);

        var message = messageResult.Value;
        
        Assert.Equal("Hello, World!", message.Content);
        Assert.Equal(conversation, message.MessageConversation);
        Assert.Equal(user1, message.SendingUser);
        
        // Check message exists in conversation
        Assert.Contains(message, conversation.Messages);
    }

    [Fact]
    public void SendMessage_With_InvalidContent()
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
        
        var messageResult = conversation.SendMessage(user1, ""); // Content cannot be null or empty
        Assert.False(messageResult.IsOk);
    }

    [Fact]
    public void SendMessage_With_InvalidUser()
    {
        var user1Result = User.CreateNewUser("TestUser1", "TestPassword1!");
        var user2Result = User.CreateNewUser("TestUser2", "TestPassword2!");
        var user3Result = User.CreateNewUser("TestUser3", "TestPassword3!");
        
        var user1 = user1Result.Value;
        var user2 = user2Result.Value;
        var user3 = user3Result.Value;
        
        Helpers.SetId(user1, Guid.NewGuid());
        Helpers.SetId(user2, Guid.NewGuid());
        Helpers.SetId(user3, Guid.NewGuid());
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());

        var messageResult = conversation.SendMessage(user3, "Valid message content");
        Assert.False(messageResult.IsOk);
    }

    [Fact]
    public void SendMessage_To_InactiveConversation()
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
        Helpers.SetProperty(conversation, nameof(IDomainObject.Active), false);
        
        var messageResult = conversation.SendMessage(user1, "Valid message content");
        Assert.False(messageResult.IsOk);
    }
}