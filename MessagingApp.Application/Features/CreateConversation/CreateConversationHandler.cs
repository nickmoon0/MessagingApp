using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.CreateConversation;

public class CreateConversationHandler : IHandler<CreateConversationCommand, CreateConversationResponse>
{
    private readonly IApplicationContext _applicationContext;

    public CreateConversationHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<CreateConversationResponse>> Handle(CreateConversationCommand request)
    {
        if (!request.ParticipantIds.Any()) 
            return new FailedToCreateEntityException("Cannot create a conversation with one user");
        if (request.ParticipantIds.Contains(request.RequestingUserId))
            return new FailedToCreateEntityException("Requesting user cannot also be a participant");
        
        var requestingUser = await _applicationContext.Users
            .Include(x => x.Friends)
            .SingleOrDefaultAsync(x => x.Id == request.RequestingUserId);

        if (requestingUser == null) return new FailedToCreateEntityException("Requesting user does not exist");

        var participants = await _applicationContext.Users
            .Where(x => request.ParticipantIds.Contains(x.Id))
            .Include(x => x.Friends)
            .ToListAsync();
        
        Conversation conversation;
        
        try
        {
            // Create a conversation based on the specified type
            switch (request.Type)
            {
                case ConversationType.DirectMessage:
                    conversation = CreateDirectMessage(requestingUser, participants);
                    break;
                case ConversationType.GroupChat:
                    return new FailedToCreateEntityException("Conversation type not yet supported");
                    break;
                default:
                    return new FailedToCreateEntityException("Unrecognised conversation type");
                    break;
            }
        }
        catch (FailedToCreateEntityException ex)
        {
            return ex;
        }

        await _applicationContext.Conversations.AddAsync(conversation);
        await _applicationContext.SaveChangesAsync();
        
        var participantSummaries = participants
            .Select(UserSummaryResponse.FromUser)
            .Append(UserSummaryResponse.FromUser(requestingUser));
        var conversationSummary = ConversationSummaryResponse.FromConversation(conversation);
        
        return new CreateConversationResponse
        {
            Conversation = conversationSummary,
            Participants = participantSummaries
        };
    }

    private static Conversation CreateDirectMessage(User requestingUser, IEnumerable<User> participants)
    {
        var user2 = participants.SingleOrDefault(x => x.Id != requestingUser.Id);
        if (user2 == null) throw new FailedToCreateEntityException("Participant does not exist");

        var conversationResult = Conversation.CreateDirectMessage(requestingUser, user2);
        if (!conversationResult.IsOk) throw conversationResult.Error;

        return conversationResult.Value;
    }
}