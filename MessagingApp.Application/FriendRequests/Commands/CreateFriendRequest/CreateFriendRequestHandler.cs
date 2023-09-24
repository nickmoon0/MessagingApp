using LanguageExt.Common;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;

public class CreateFriendRequestHandler : IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>
{
    public Task<Result<CreateFriendRequestResponse>> Handle(CreateFriendRequestCommand req)
    {
        throw new NotImplementedException();
    }
}