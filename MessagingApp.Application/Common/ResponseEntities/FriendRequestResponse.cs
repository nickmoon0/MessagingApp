using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.ResponseEntities;

public class FriendRequestResponse
{
    public Guid? FriendRequestId { get; init; }
    public UserSummaryResponse? SendingUser { get; init; }
    public UserSummaryResponse? ReceivingUser { get; init; }
    public FriendRequestStatus? Status { get; init; }
    
    public static FriendRequestResponse FromFriendRequest(FriendRequest request)
    {
        return new FriendRequestResponse
        {
            FriendRequestId = request.Id,
            Status = request.Status,
            SendingUser = new UserSummaryResponse
            {
                Id = request.SendingUser?.Id,
                Username = request.SendingUser?.Username
            },
            ReceivingUser = new UserSummaryResponse
            {
                Id = request.ReceivingUser?.Id,
                Username = request.ReceivingUser?.Username
            }
        };
    }
}