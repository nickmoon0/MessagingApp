using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.RetrievePendingFriendRequests;

public class RetrievePendingFriendRequestsQuery : IRequest<RetrievePendingFriendRequestsResponse>
{
    public Guid UserId { get; set; }

    public RetrievePendingFriendRequestsQuery(Guid userId)
    {
        UserId = userId;
    }
}