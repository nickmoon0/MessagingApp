using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.CreateFriendRequest;

public class CreateFriendRequestHandler : BaseHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>
{
    private readonly IUserRepository _userRepository;
    public CreateFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<Result<CreateFriendRequestResponse>> HandleRequest(CreateFriendRequestCommand request)
    {
        var friendRequest = new FriendRequest(request.FromUser, request.ToUser, FriendRequestStatus.Pending);
        var fromUser = await _userRepository.GetUserById(request.FromUser);
        var toUser = await _userRepository.GetUserById(request.ToUser);

        if (fromUser == null || toUser == null)
        {
            var notFoundException = new EntityNotFoundException("User could not be found");
            return new Result<CreateFriendRequestResponse>(notFoundException);
        }

        fromUser.SendFriendRequest(friendRequest, request.RequestingUser);
        await _userRepository.UpdateUser(fromUser);
        return new Result<CreateFriendRequestResponse>(new CreateFriendRequestResponse());
    }
}