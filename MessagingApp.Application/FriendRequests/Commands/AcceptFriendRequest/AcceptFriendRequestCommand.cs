using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.FriendRequests.Commands.AcceptFriendRequest;

public class AcceptFriendRequestCommand : IRequest<AcceptFriendRequestResponse>
{
    public Guid FriendRequestId { get; set; }
    public Guid ToUserId { get; set; }
    public Guid RequestingUserId { get; set; }

    public AcceptFriendRequestCommand(AcceptFriendRequestRequest request, Guid requestingUserId)
    {
        FriendRequestId = request.FriendRequestId;
        ToUserId = requestingUserId;
        RequestingUserId = requestingUserId;
    }
}