using System.Collections.Immutable;
using MessagingApp.Application.Features.GetAllConversations;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class GetAllConversationsTests
{
    [Fact]
    public async Task GetConversations_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversationResult.Value);
        await applicationContext.SaveChangesAsync();

        var conversationId = conversationResult.Value.Id;
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetAllConversationsHandler(applicationContext);
        var query = new GetAllConversationsQuery { UserId = user1.Id };
        
        var handlerResult = await handler.Handle(query);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;

        var createdConversationPresent = response.Conversations.Any(x => x.Id == conversationId);
        
        Assert.Single(response.Conversations);
        Assert.True(createdConversationPresent);
    }

    [Fact]
    public async Task GetConversations_With_NonExistentUser()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        
        var conversationResult = Conversation.CreateDirectMessage(user1, user2);

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversationResult.Value);
        await applicationContext.SaveChangesAsync();
        
        applicationContext.ChangeTracker.Clear();

        var handler = new GetAllConversationsHandler(applicationContext);
        var query = new GetAllConversationsQuery { UserId = Guid.NewGuid() };
        
        var handlerResult = await handler.Handle(query);
        Assert.False(handlerResult.IsOk);
    }
}