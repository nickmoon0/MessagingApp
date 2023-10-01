using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.RetrieveConversation;

public class RetrieveConversationQuery : IRequest<RetrieveConversationResponse>
{
    public Guid RequestingUserId { get; set; }
    public Guid UserId { get; set; }

    public RetrieveConversationQuery(Guid requestingUserId, Guid userId)
    {
        RequestingUserId = requestingUserId;
        UserId = userId;
    }
}