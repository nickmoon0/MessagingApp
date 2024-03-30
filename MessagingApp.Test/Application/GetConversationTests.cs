using MessagingApp.Application.Features.GetConversation;
using MessagingApp.Domain.Aggregates;
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

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.SaveChangesAsync();

        var conversationId = conversation.Value.Id;
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetConversationHandler(applicationContext);
        var query = new GetConversationQuery { UserId = user1.Id, ConversationId = conversationId };

        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;

        var containsUser1 = response.Participants.Any(x => x.Id == user1.Id);
        var containsUser2 = response.Participants.Any(x => x.Id == user2.Id);
        
        Assert.Equal(conversationId, response.Id);
        Assert.Equal(2, response.Participants.Count());
        Assert.True(containsUser1);
        Assert.True(containsUser2);
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