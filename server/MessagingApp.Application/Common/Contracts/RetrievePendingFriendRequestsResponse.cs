using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Contracts;

public class RetrievePendingFriendRequestsResponse
{
    public IEnumerable<FriendRequestResponse> FriendRequests { get; set; }

    public RetrievePendingFriendRequestsResponse(IEnumerable<FriendRequest> requests)
    {
        FriendRequests = ParseFriendRequests(requests);
    }

    private static IEnumerable<FriendRequestResponse> ParseFriendRequests(IEnumerable<FriendRequest> requests) => 
        requests.Select(request => 
            new FriendRequestResponse
            {
                FriendRequestId = request.Id,
                ToUserId = request.ToUserId, 
                FromUserId = request.FromUserId, 
                RequestDate = request.RequestDate
            }).ToList();

    public class FriendRequestResponse
    {
        public required Guid FriendRequestId { get; set; }
        public required Guid ToUserId { get; set; }

        public required Guid FromUserId { get; set; }
        
        public required DateTime RequestDate { get; set; }
        
    }
}