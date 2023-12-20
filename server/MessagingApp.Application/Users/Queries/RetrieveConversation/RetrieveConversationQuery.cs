using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.Users.Queries.RetrieveConversation;

public class RetrieveConversationQuery : IRequest<Result<RetrieveConversationResponse>>
{
    public Guid RequestingUserId { get; set; }
    public Guid UserId { get; set; }

    public RetrieveConversationQuery(Guid requestingUserId, Guid userId)
    {
        RequestingUserId = requestingUserId;
        UserId = userId;
    }
}