using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Services;

namespace MessagingApp.Application.Users.Commands.AcceptFriendRequest;

public class AcceptFriendRequestHandler : IHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>
{
    private readonly IUserService _userService;

    public AcceptFriendRequestHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<AcceptFriendRequestResponse>> Handle(AcceptFriendRequestCommand req)
    {
        try
        {
            var friendRequest = await _userService.GetFriendRequestById(req.FriendRequestId);
            var toUser = await _userService.GetUserById(req.ToUserId);

            if (friendRequest == null || toUser == null)
            {
                var notFoundEx = new EntityNotFoundException("Could not find entities when attempting to accept friend request");
                return new Result<AcceptFriendRequestResponse>(notFoundEx);
            }
            
            var fromUser = await _userService.GetUserById(friendRequest.FromUserId);
            if (fromUser == null)
            {
                var notFoundEx = new EntityNotFoundException("Sending user does not exist");
                return new Result<AcceptFriendRequestResponse>(notFoundEx);
            }
            
            toUser.AcceptFriendRequest(friendRequest, req.RequestingUserId);
            fromUser.AddFriend(toUser.Id);
            
            await _userService.UpdateUser(toUser);
            await _userService.UpdateUser(fromUser);
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