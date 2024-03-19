using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Helpers;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.FriendRequestFeatures.AcceptFriendRequest;

public class AcceptFriendRequestHandler : 
    IRequestHandler<AcceptFriendRequestCommand, Result<AcceptFriendRequestResponse>>
{
    private readonly IUserRepository _userRepository;

    public AcceptFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<AcceptFriendRequestResponse>> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
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
        
        var acceptResult = toUser.AcceptFriendRequest(friendRequest, request.RequestingUserId);
        if (!acceptResult.Success)
            return new Result<AcceptFriendRequestResponse>(ExceptionHelper.ResolveException(acceptResult.Error!));
        
        var addResult = fromUser.AddFriend(toUser.Id);
        if (!addResult.Success)
            return new Result<AcceptFriendRequestResponse>(ExceptionHelper.ResolveException(addResult.Error!));
        
        await _userRepository.UpdateUser(toUser);
        await _userRepository.UpdateUser(fromUser);

        if (fromUser.Username == null) throw new Exception("Username could not be found");
        var response = new AcceptFriendRequestResponse(fromUser.Id, fromUser.Username);
        return new Result<AcceptFriendRequestResponse>(response);
    }
}