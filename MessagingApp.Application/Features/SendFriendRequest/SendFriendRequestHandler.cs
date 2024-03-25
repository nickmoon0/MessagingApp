using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.SendFriendRequest;

public class SendFriendRequestHandler : IHandler<SendFriendRequestCommand, SendFriendRequestResponse>
{
    private readonly IApplicationContext _applicationContext;

    public SendFriendRequestHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<SendFriendRequestResponse>> Handle(SendFriendRequestCommand request)
    {
        // Retrieve user objects
        var sendingUser = await _applicationContext.Users
            .Include(x => x.SentFriendRequests)
            .SingleOrDefaultAsync(x => x.Id == request.SendingUserId);
        var receivingUser = await _applicationContext.Users
            .Include(x => x.SentFriendRequests)
            .SingleOrDefaultAsync(x => x.Id == request.ReceivingUserId);
        
        if (sendingUser == null) return new FailedToRetrieveEntityException("Sending user does not exist");
        if (receivingUser == null) return new FailedToRetrieveEntityException("Receiving user does not exist");
        
        // Create friend request and ensure domain rules are met
        var friendRequestResult = sendingUser.SendFriendRequest(receivingUser);
        if (!friendRequestResult.IsOk) return friendRequestResult.Error;
        var friendRequest = friendRequestResult.Value;

        await _applicationContext.FriendRequests.AddAsync(friendRequest);
        await _applicationContext.SaveChangesAsync();

        return new SendFriendRequestResponse
        {
            Id = friendRequest.Id,
            SendingUserId = friendRequest.SendingUser!.Id,
            ReceivingUserId = friendRequest.ReceivingUser!.Id
        };
    }
}