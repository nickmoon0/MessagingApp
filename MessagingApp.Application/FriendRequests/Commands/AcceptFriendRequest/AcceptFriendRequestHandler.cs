using LanguageExt.Common;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.FriendRequests.Commands.AcceptFriendRequest;

public class AcceptFriendRequestHandler : IHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>
{
    public Task<Result<AcceptFriendRequestResponse>> Handle(AcceptFriendRequestCommand req)
    {
        throw new NotImplementedException();
    }
}