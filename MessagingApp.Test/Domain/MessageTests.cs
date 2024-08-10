using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Domain;

public class MessageTests
{
    [Fact]
    public void CreateMessage_With_ValidContent()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        var message = Message.CreateMessage(user1, conversation, "Valid Content");
        
        Assert.True(message.IsOk);
    }
    
    [Fact]
    public void CreateMessage_With_EmptyContent()
    {
        var user1 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser1");
        var user2 = DomainObjectFactory.CreateUser(id: Guid.NewGuid(), username: "TestUser2");
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);
        var conversation = conversationResult.Value;
        
        Helpers.SetId(conversation, Guid.NewGuid());
        var message = Message.CreateMessage(user1, conversation, "");
        
        Assert.False(message.IsOk);
    }
}