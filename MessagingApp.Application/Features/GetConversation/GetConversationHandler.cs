using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.GetConversation;

public class GetConversationHandler : IHandler<GetConversationQuery, GetConversationResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetConversationHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetConversationResponse>> Handle(GetConversationQuery request)
    {
        var conversation = await _applicationContext.Conversations
            .Include(x => x.Participants)
            .Include(x => x.Messages)
            .SingleOrDefaultAsync(x => x.Id == request.ConversationId);
        if (conversation == null) return new FailedToRetrieveEntityException("Conversation does not exist");

        // Check that the user requesting the conversation is part of it
        if (conversation.Participants.All(x => x.Id != request.UserId))
            return new InvalidCredentialsException("User is not part of conversation");
        
        var messagesToRetrieve = request.MessagesToRetrieve ?? 20; // Default to 20
        
        var participants = conversation.Participants.Select(UserSummaryResponse.FromUser);
        var messages = conversation.Messages
            .OrderByDescending(x => x.TimeStamp)
            .Take(messagesToRetrieve)
            .Select(MessageResponse.FromMessage);
        
        return new GetConversationResponse
        {
            Id = conversation.Id,
            Name = conversation.Name,
            Type = conversation.Type,
            Participants = participants,
            Messages = messages
        };
    }
}