using System.Security.Claims;
using MessagingApp.Application.Common;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Application.Features.GetAllConversations;
using MessagingApp.Application.Features.SendMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingApp.Api.Hubs;

[Authorize]
public class ConversationHub : Hub<IConversationClient>
{
    private readonly ILogger<ConversationHub> _logger;
    private readonly IHandler<SendMessageCommand, SendMessageResponse> _sendMessageHandler;
    private readonly IHandler<GetAllConversationsQuery, GetAllConversationsResponse> _getConversationsHandler;
    
    private string? Username => Context.User?.Claims
        .SingleOrDefault(x => x.Type == ClaimTypes.Name)?
        .Value;
    
    public ConversationHub(
        ILogger<ConversationHub> logger,
        IHandler<SendMessageCommand, SendMessageResponse> sendMessageHandler,
        IHandler<GetAllConversationsQuery, GetAllConversationsResponse> getConversationsHandler)
    {
        _logger = logger;
        _sendMessageHandler = sendMessageHandler;
        _getConversationsHandler = getConversationsHandler;
    }
    
    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier == null)
        {
            _logger.LogInformation("User was disconnected because {UserIdClaim} was null", nameof(Context.UserIdentifier));
            Context.Abort();
            return;
        }

        if (!Guid.TryParse(Context.UserIdentifier, out var userId))
        {
            _logger.LogInformation("User was disconnected because {UserIdClaim} could not be parsed to a Guid (value: {Value})", 
                nameof(Context.UserIdentifier),
                Context.UserIdentifier);
            Context.Abort();
            return;
        }
        
        _logger.LogInformation("User {Username} ({UserId}) connected. Adding user to groups", Username, userId);
        var successfullyAdded = await AddUserToGroups(userId);

        if (!successfullyAdded)
        {
            _logger.LogError("Failed to add {Username} ({UserId}) to groups. Terminating connection.", 
                Username, userId);
            Context.Abort();
            return;
        }
    }
    
    /// <summary>
    /// Endpoint 
    /// </summary>
    /// <param name="conversationId"></param>
    /// <param name="messageContent"></param>
    public async Task SendMessage(Guid conversationId, string messageContent)
    {
        var command = new SendMessageCommand
        {
            // Can suppress null warnings because userID is checked on connection
            SendingUserId = Guid.Parse(Context.UserIdentifier!),
            ConversationId = conversationId,
            Content = messageContent
        };
        
        var messageResponse = await _sendMessageHandler.Handle(command);
        if (!messageResponse.IsOk)
        {
            _logger.LogInformation("{Username} ({UserId}) failed to send a message for reason: {Reason}",
                Username, Context.UserIdentifier, messageResponse.Error.Message);
            await Clients.Clients(Context.ConnectionId).NotifyUser("Message failed to send");
            return;
        }

        var response = messageResponse.Value;
        
        await Clients.Group(conversationId.ToString()).ReceiveMessage(response.Message);
        
        _logger.LogInformation("Send message {MessageId} to conversation {ConversationId}",
            response.Message.Id, conversationId);
    }
    
    private async Task<bool> AddUserToGroups(Guid userId)
    {
        var query = new GetAllConversationsQuery { UserId = userId };
        var conversationsResult = await _getConversationsHandler.Handle(query);
        if (!conversationsResult.IsOk) return false;

        var response = conversationsResult.Value;
        var tasksList = response.Conversations
            .Select(conversation => Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString()))
            .ToList();

        await Task.WhenAll(tasksList);
        return true;
    }
}

public interface IConversationClient
{
    public Task ReceiveMessage(MessageResponse message);
    
    /// <summary>
    /// Used to notify the user of the status of an action. Typically used when an action failed (e.g., failed to send
    /// a message).
    /// </summary>
    /// <param name="message">Message that will be sent to client</param>
    /// <returns></returns>
    public Task NotifyUser(string message);
}