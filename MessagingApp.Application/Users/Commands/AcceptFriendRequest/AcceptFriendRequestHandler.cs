using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Commands.AcceptFriendRequest;

public class AcceptFriendRequestHandler : BaseHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>
{
    private readonly IUserRepository _userRepository;

    public AcceptFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    protected override async Task<Result<AcceptFriendRequestResponse>> HandleRequest(AcceptFriendRequestCommand request)
    {
        var friendRequest = await _userRepository.GetFriendRequestById(request.FriendRequestId);
        var toUser = await _userRepository.GetUserById(request.ToUserId);

        if (friendRequest == null || toUser == null)
        {
            var notFoundEx =
                new EntityNotFoundException("Could not find entities when attempting to accept friend request");
            return new Result<AcceptFriendRequestResponse>(notFoundEx);
        }

        var fromUser = await _userRepository.GetUserById(friendRequest.FromUserId);
        if (fromUser == null)
        {
            var notFoundEx = new EntityNotFoundException("Sending user does not exist");
            return new Result<AcceptFriendRequestResponse>(notFoundEx);
        }

        toUser.AcceptFriendRequest(friendRequest, request.RequestingUserId);
        fromUser.AddFriend(toUser.Id);

        await _userRepository.UpdateUser(toUser);
        await _userRepository.UpdateUser(fromUser);
        return new Result<AcceptFriendRequestResponse>(new AcceptFriendRequestResponse());
    }
}