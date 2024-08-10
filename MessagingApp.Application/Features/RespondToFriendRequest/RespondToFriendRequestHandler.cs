using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.RespondToFriendRequest;

public class RespondToFriendRequestHandler : IHandler<RespondToFriendRequestCommand, RespondToFriendRequestResponse>
{
    private readonly IApplicationContext _applicationContext;
    
    public RespondToFriendRequestHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    public async Task<Result<RespondToFriendRequestResponse>> Handle(RespondToFriendRequestCommand request)
    {
        var respondingUser = await _applicationContext.Users
            .Include(x => x.ReceivedFriendRequests)
            .SingleOrDefaultAsync(x => x.Id == request.RespondingUserId);
        var friendRequest = await _applicationContext.FriendRequests
            .Include(x => x.SendingUser)
            .Include(x => x.ReceivingUser)
            .SingleOrDefaultAsync(x => x.Id == request.FriendRequestId);

        if (respondingUser == null) return new FailedToRetrieveEntityException("Responding user does not exist");
        if (friendRequest == null) return new FailedToRetrieveEntityException("Friend request does not exist");

        var friendRequestResult = respondingUser.RespondToFriendRequest(friendRequest, request.Response);
        if (!friendRequestResult.IsOk) return friendRequestResult.Error;

        await _applicationContext.SaveChangesAsync();

        return new RespondToFriendRequestResponse
        {
            FriendRequestId = friendRequest.Id,
            NewStatus = friendRequest.Status
        };
    }
}