using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.FriendRequests.Commands.CreateFriendRequest;

public class CreateFriendRequestHandler : IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>
{
    private readonly IUserRepository _userRepository;
    public CreateFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Result<CreateFriendRequestResponse>> Handle(CreateFriendRequestCommand req)
    {
        try
        {
            var friendRequest = new FriendRequest(req.FromUser, req.ToUser);
            var fromUser = await _userRepository.GetUserById(req.FromUser);
            var toUser = await _userRepository.GetUserById(req.ToUser);

            if (fromUser == null || toUser == null)
            {
                var notFoundException = new EntityNotFoundException("User could not be found");
                return new Result<CreateFriendRequestResponse>(notFoundException);
            }

            fromUser.SendFriendRequest(friendRequest, req.RequestingUser);
            await _userRepository.UpdateUser(fromUser);
            return new Result<CreateFriendRequestResponse>(new CreateFriendRequestResponse());
        }
        catch (InvalidOperationException ex)
        {
            return new Result<CreateFriendRequestResponse>(ex);
        }
        catch (Exception ex)
        {
            return new Result<CreateFriendRequestResponse>(ex);
        }
    }
}