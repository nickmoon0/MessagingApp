using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.FriendRequestFeatures.AcceptFriendRequest;

public class AcceptFriendRequestCommand : IRequest<Result<AcceptFriendRequestResponse>>
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