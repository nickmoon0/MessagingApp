using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.GetAllConversations;

public class GetAllConversationsHandler : IHandler<GetAllConversationsQuery, GetAllConversationsResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetAllConversationsHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetAllConversationsResponse>> Handle(GetAllConversationsQuery request)
    {
        // Retrieve the user with all their conversations
        var user = await _applicationContext.Users
            .Include(x => x.Conversations)
            .SingleOrDefaultAsync(x => x.Id == request.UserId);
        
        if (user == null) return new FailedToRetrieveEntityException("User does not exist");

        // Put conversations into response
        var conversationEnumerable = user.Conversations.AsEnumerable();
        var conversations = conversationEnumerable
            .Select(ConversationResponse.ConversationResponseFromConversation)
            .ToList();

        return new GetAllConversationsResponse { Conversations = conversations };
    }
}