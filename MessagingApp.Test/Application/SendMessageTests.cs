using MessagingApp.Application.Features.SendMessage;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Infrastructure.Data;
using MessagingApp.Test.Common;

namespace MessagingApp.Test.Application;

public class SendMessageTests
{
    [Fact]
    public async Task SendMessage_With_ValidParameters()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        
        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var conversation = Conversation.CreateDirectMessage(user1, user2);

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new SendMessageHandler(applicationContext);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        var conversationId = conversation.Value.Id;

        applicationContext.ChangeTracker.Clear();
        
        var command = new SendMessageCommand
        {
            SendingUserId = user1Id,
            ConversationId = conversationId,
            Content = "Test message content"
        };
        
        var handlerResult = await handler.Handle(command);
        Assert.True(handlerResult.IsOk);

        var response = handlerResult.Value;
        
        Assert.Equal(command.Content, response.Message.Content);
        Assert.Equal(user1.Id, response.Message.SendingUser.Id);
    }
    
    [Fact]
    public async Task SendMessage_With_NonExistentUser()
    {
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new SendMessageHandler(applicationContext);
        
        var command = new SendMessageCommand
        {
            SendingUserId = Guid.NewGuid(),
            ConversationId = Guid.NewGuid(),
            Content = "Test message content"
        };
        
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("User does not exist", handlerResult.Error.Message);
    }
    
    [Fact]
    public async Task SendMessage_With_NonExistentConversation()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        
        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new SendMessageHandler(applicationContext);

        await applicationContext.AddAsync(user1);
        await applicationContext.SaveChangesAsync();
        
        var user1Id = user1.Id;
        
        applicationContext.ChangeTracker.Clear();
        
        var command = new SendMessageCommand
        {
            SendingUserId = user1Id,
            ConversationId = Guid.NewGuid(),
            Content = "Test message content"
        };
        
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("Conversation does not exist", handlerResult.Error.Message);
    }
    
    [Fact]
    public async Task SendMessage_With_NoMessageContent()
    {
        var user1 = DomainObjectFactory.CreateUser(username: "TestUser1", id: Guid.NewGuid());
        var user2 = DomainObjectFactory.CreateUser(username: "TestUser2", id: Guid.NewGuid());
        
        Helpers.SetProperty(user1, nameof(user1.Friends), new List<User> { user2 });
        Helpers.SetProperty(user2, nameof(user2.Friends), new List<User> { user1 });
        
        var conversation = Conversation.CreateDirectMessage(user1, user2);

        var applicationContext = new ApplicationContext(DatabaseSetup.Options);
        var handler = new SendMessageHandler(applicationContext);

        await applicationContext.AddRangeAsync(user1, user2);
        await applicationContext.AddAsync(conversation.Value);
        await applicationContext.SaveChangesAsync();

        var user1Id = user1.Id;
        var conversationId = conversation.Value.Id;

        applicationContext.ChangeTracker.Clear();
        
        var command = new SendMessageCommand
        {
            SendingUserId = user1Id,
            ConversationId = conversationId,
            Content = ""
        };
        
        var handlerResult = await handler.Handle(command);
        Assert.False(handlerResult.IsOk);
        Assert.Equal("Message content cannot be empty", handlerResult.Error.Message);
    }
}