using MessagingApp.Application.Features.CreateConversation;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Test.Application;

public class CreateConversationTests
{
    [Fact]
    public async Task CreateDirectMessage_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());

        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new CreateConversationHandler(applicationContext);
        
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        var user2Id = user2.Id;
        
        applicationContext.ChangeTracker.Clear();

        var command = new CreateConversationCommand
        {
            Type = ConversationType.DirectMessage,
            RequestingUserId = user1Id,
            ParticipantIds = [ user2Id ]
        };

        var handlerResult = await handler.Handle(command);
        Assert.True(handlerResult.IsOk);
        
        // Assert that a single conversation was created in database
        var allConversations = await applicationContext.Conversations.ToListAsync();
        Assert.Single(allConversations);
    }

    [Fact]
    public async Task CreateConversation_With_NoParticipants()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new CreateConversationHandler(applicationContext);
        
        await applicationContext.Users.AddAsync(user1);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        
        applicationContext.ChangeTracker.Clear();

        var command = new CreateConversationCommand
        {
            Type = ConversationType.DirectMessage,
            RequestingUserId = user1Id,
            ParticipantIds = []
        };

        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("Cannot create a conversation with one user", handlerResult.Error.Message);

        // Assert that conversation was never created in database
        var allConversations = await applicationContext.Conversations.ToListAsync();
        Assert.Empty(allConversations);
    }
    
    [Fact]
    public async Task CreateConversation_With_RequestingUserAsParticipant()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());

        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new CreateConversationHandler(applicationContext);
        
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        var user2Id = user2.Id;
        
        applicationContext.ChangeTracker.Clear();

        var command = new CreateConversationCommand
        {
            Type = ConversationType.DirectMessage,
            RequestingUserId = user1Id,
            ParticipantIds = [ user1Id, user2Id ]
        };

        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("Requesting user cannot also be a participant", handlerResult.Error.Message);
        
        // Assert that conversation was never created in database
        var allConversations = await applicationContext.Conversations.ToListAsync();
        Assert.Empty(allConversations);
    }
    
    [Fact]
    public async Task CreateConversation_With_InvalidType()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());

        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new CreateConversationHandler(applicationContext);
        
        await applicationContext.Users.AddRangeAsync(user1, user2);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        var user2Id = user2.Id;
        
        applicationContext.ChangeTracker.Clear();

        var command = new CreateConversationCommand
        {
            Type = (ConversationType)2,
            RequestingUserId = user1Id,
            ParticipantIds = [ user2Id ]
        };

        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("Unrecognised conversation type", handlerResult.Error.Message);
        
        // Assert that conversation was never created in database
        var allConversations = await applicationContext.Conversations.ToListAsync();
        Assert.Empty(allConversations);
    }
}