using MessagingApp.Application.Features.GetConversation;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetConversationTests
{
    [Fact]
    public async Task GetConversation_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        var conversation = Conversation.CreateDirectMessage(user1, user2);
        var message = Message.CreateMessage(user1, conversation.Value, "Test Message");

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.AddAsync(message.Value);
        await applicationContext.SaveChangesAsync();
        
        var conversationId = conversation.Value.Id;
        var messageId = message.Value.Id;
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetConversationHandler(applicationContext);
        var query = new GetConversationQuery { UserId = user1.Id, ConversationId = conversationId };

        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;

        var containsUser1 = response.Participants.Any(x => x.Id == user1.Id);
        var containsUser2 = response.Participants.Any(x => x.Id == user2.Id);

        var containsMessage = response.Messages.Any(x => x.Id == messageId);
        
        Assert.Equal(conversationId, response.Id);
        Assert.Equal(2, response.Participants.Count());
        Assert.Single(response.Messages);
        Assert.True(containsUser1);
        Assert.True(containsUser2);
        Assert.True(containsMessage);
    }

    [Fact]
    public async Task GetConversation_With_LimitedMessages()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        var conversation = Conversation.CreateDirectMessage(user1, user2);
        var message1 = Message.CreateMessage(user1, conversation.Value, "Test Message 1");
        var message2 = Message.CreateMessage(user1, conversation.Value, "Test Message 2");
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.AddRangeAsync(message1.Value, message2.Value);
        await applicationContext.SaveChangesAsync();
        
        var conversationId = conversation.Value.Id;
        var message2Id = message2.Value.Id;
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetConversationHandler(applicationContext);
        var query = new GetConversationQuery
        {
            UserId = user1.Id,
            ConversationId = conversationId,
            MessagesToRetrieve = 1
        };

        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;
        var message2Present = response.Messages.Any(x => x.Id == message2Id);
        
        Assert.Single(response.Messages);
        Assert.True(message2Present);
    }
    
    [Fact]
    public async Task GetConversation_With_NonExistentConversation()
    {
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetConversationHandler(applicationContext);
        var query = new GetConversationQuery { UserId = Guid.NewGuid(), ConversationId = Guid.NewGuid() };

        var handlerResult = await handler.Handle(query);
        Assert.False(handlerResult.IsOk);
    }

    [Fact]
    public async Task GetConversation_With_UserWhoIsNotParticipant()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        
        var conversation = Conversation.CreateDirectMessage(user1, user2);

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.SaveChangesAsync();

        var conversationId = conversation.Value.Id;
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetConversationHandler(applicationContext);
        var query = new GetConversationQuery { UserId = Guid.NewGuid(), ConversationId = conversationId };

        var handlerResult = await handler.Handle(query);
        Assert.False(handlerResult.IsOk);
    }
}