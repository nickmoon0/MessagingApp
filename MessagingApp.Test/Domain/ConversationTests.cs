using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class ConversationTests
{
    [Fact]
    public void CreateDirectMessage_With_ValidUsers()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        Assert.True(conversationResult.IsOk);

        var conversation = conversationResult.Value;
        
        Assert.Null(conversation.Name);
        Assert.Equal(ConversationType.DirectMessage, conversation.Type);
    }

    [Fact]
    public void CreateDirectMessage_With_SameUser()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var conversationResult = Conversation.CreateDirectMessage(user1, user1);
        
        Assert.False(conversationResult.IsOk);
    }

    [Fact]
    public void SendMessage_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
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
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        
        var messageResult = conversation.SendMessage(user1, ""); // Content cannot be null or empty
        Assert.False(messageResult.IsOk);
    }

    [Fact]
    public void SendMessage_With_InvalidUser()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");;
        var user3 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser3");;
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());

        var messageResult = conversation.SendMessage(user3, "Valid message content");
        Assert.False(messageResult.IsOk);
    }

    [Fact]
    public void SendMessage_To_InactiveConversation()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        Helpers.SetProperty(conversation, nameof(IPersistedObject.Active), false);
        
        var messageResult = conversation.SendMessage(user1, "Valid message content");
        Assert.False(messageResult.IsOk);
    }
}