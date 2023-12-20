using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.Users.Queries.RetrievePendingFriendRequests;

public class RetrievePendingFriendRequestsQuery : IRequest<Result<RetrievePendingFriendRequestsResponse>>
{
    public Guid UserId { get; set; }

    public RetrievePendingFriendRequestsQuery(Guid userId)
    {
        UserId = userId;
    }
}