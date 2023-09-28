using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.CreateFriendRequest;

public class CreateFriendRequestHandler : IHandler<CreateFriendRequestCommand, CreateFriendRequestResponse>
{
    private readonly IUserService _userService;
    public CreateFriendRequestHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result<CreateFriendRequestResponse>> Handle(CreateFriendRequestCommand req)
    {
        try
        {
            var friendRequest = new FriendRequest(req.FromUser, req.ToUser, FriendRequestStatus.Pending);
            var fromUser = await _userService.GetUserById(req.FromUser);
            var toUser = await _userService.GetUserById(req.ToUser);

            if (fromUser == null || toUser == null)
            {
                var notFoundException = new EntityNotFoundException("User could not be found");
                return new Result<CreateFriendRequestResponse>(notFoundException);
            }

            fromUser.SendFriendRequest(friendRequest, req.RequestingUser);
            await _userService.UpdateUser(fromUser);
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