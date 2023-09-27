using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.FriendRequests.Commands.AcceptFriendRequest;

public class AcceptFriendRequestHandler : IHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;

    public AcceptFriendRequestHandler(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository)
    {
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
    }

    public async Task<Result<AcceptFriendRequestResponse>> Handle(AcceptFriendRequestCommand req)
    {
        try
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestById(req.FriendRequestId);
            var toUser = await _userRepository.GetUserById(req.ToUserId);

            if (friendRequest == null || toUser == null)
            {
                var notFoundEx = new EntityNotFoundException("Could not find entities when attempting to accept friend request");
                return new Result<AcceptFriendRequestResponse>(notFoundEx);
            }
            
            var fromUser = await _userRepository.GetUserById(friendRequest.FromUserId);
            if (fromUser == null)
            {
                var notFoundEx = new EntityNotFoundException("Sending user does not exist");
                return new Result<AcceptFriendRequestResponse>(notFoundEx);
            }
            
            toUser.AcceptFriendRequest(friendRequest, req.RequestingUserId);
            fromUser.AddFriend(toUser.Id);
            
            await _userRepository.UpdateUser(toUser);
            await _userRepository.UpdateUser(fromUser);
            return new Result<AcceptFriendRequestResponse>(new AcceptFriendRequestResponse());
        }
        catch (InvalidOperationException ex)
        {
            return new Result<AcceptFriendRequestResponse>(ex);
        }
        catch (Exception ex)
        {
            return new Result<AcceptFriendRequestResponse>(ex);
        }
    }
}