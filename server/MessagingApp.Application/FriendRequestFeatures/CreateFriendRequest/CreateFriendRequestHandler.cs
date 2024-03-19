using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Helpers;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Common;

namespace MessagingApp.Application.FriendRequestFeatures.CreateFriendRequest;

public class CreateFriendRequestHandler : IRequestHandler<CreateFriendRequestCommand, Result<CreateFriendRequestResponse>>
{
    private readonly IUserRepository _userRepository;
    public CreateFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<CreateFriendRequestResponse>> Handle(CreateFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var friendRequest = new Domain.Entities.FriendRequest(request.FromUser, request.ToUser, FriendRequestStatus.Pending);
        var fromUser = await _userRepository.GetUserById(request.FromUser);
        var toUser = await _userRepository.GetUserById(request.ToUser);

        if (fromUser == null || toUser == null)
        {
            var notFoundException = new EntityNotFoundException("User could not be found");
            return new Result<CreateFriendRequestResponse>(notFoundException);
        }

        var result = fromUser.SendFriendRequest(friendRequest, request.RequestingUser);
        if (!result.Success)
            return new Result<CreateFriendRequestResponse>(ExceptionHelper.ResolveException(result.Error!));
        
        await _userRepository.UpdateUser(fromUser);

        var response = new CreateFriendRequestResponse(friendRequest.Id, toUser.Id);
        return new Result<CreateFriendRequestResponse>(response);
    }
}