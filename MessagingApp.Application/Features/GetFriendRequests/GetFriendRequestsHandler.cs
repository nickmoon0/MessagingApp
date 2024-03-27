using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.GetFriendRequests;

public class GetFriendRequestsHandler : IHandler<GetFriendRequestsQuery, GetFriendRequestsResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetFriendRequestsHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetFriendRequestsResponse>> Handle(GetFriendRequestsQuery request)
    {
        var query = _applicationContext.Users.AsQueryable();

        if (request.GetReceivedRequests)
            query = query
                .Include(x => x.ReceivedFriendRequests);
        if (request.GetSentRequests)
            query = query
                .Include(x => x.SentFriendRequests)
                .ThenInclude(x => x.ReceivingUser);
    
        var user = await query.SingleOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) return new FailedToRetrieveEntityException("User does not exist");
        
        var sentRequestsEnumerable = user.SentFriendRequests.AsEnumerable();
        var receivedRequestsEnumerable = user.ReceivedFriendRequests.AsEnumerable();
        
        // Filter by status if required
        if (request.Status != null)
        {
            sentRequestsEnumerable = sentRequestsEnumerable.Where(x => x.Status == request.Status);
            receivedRequestsEnumerable = receivedRequestsEnumerable.Where(x => x.Status == request.Status);
        }
        
        var sentRequests = sentRequestsEnumerable
            .Select(FriendRequestResponse.FromFriendRequest)
            .ToList();

        var receivedRequests = receivedRequestsEnumerable
            .Select(FriendRequestResponse.FromFriendRequest)
            .ToList();

        return new GetFriendRequestsResponse
        {
            SentFriendRequests = sentRequests,
            ReceivedFriendRequests = receivedRequests
        };
    }

}