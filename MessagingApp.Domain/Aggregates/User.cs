using FluentValidation.Results;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;
using MessagingApp.Domain.Validators;

namespace MessagingApp.Domain.Aggregates;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }

    public string? Password { get; set; }

    public List<UserFriend> Friends { get; set; } = new();
    public List<FriendRequest> SentFriendRequests { get; set; } = new();
    public List<FriendRequest> ReceivedFriendRequests { get; set; } = new();

    public void SendFriendRequest(FriendRequest request, Guid requestingUser)
    {
        // Ensures the request is valid
        var validator = new SendFriendRequestValidator(requestingUser, SentFriendRequests, ReceivedFriendRequests);
        var valResult = validator.Validate(request);
        
        if (!valResult.IsValid)
        {
            throw new InvalidOperationException();
        }
        
        SentFriendRequests.Add(request);
    }

    public void AcceptFriendRequest(FriendRequest request, Guid requestingUser)
    {
        var validator = new AcceptFriendRequestValidator(requestingUser, ReceivedFriendRequests);
        var valResult = validator.Validate(request);

        if (!valResult.IsValid)
        {
            throw new InvalidOperationException();
        }

        // Get friend request and change status to accepted
        var storedReq = ReceivedFriendRequests.Single(x => x.Id == request.Id);
        storedReq.Status = FriendRequestStatus.Accepted;
        
        // Create friend and add to friends list 
        var userFriend = new UserFriend
        {
            UserId = request.ToUserId,
            FriendId = request.FromUserId
        };
        
        Friends.Add(userFriend);
    }

    public void AddFriend(Guid userToAdd)
    {
        var userFriend = new UserFriend
        {
            UserId = Id,
            FriendId = userToAdd
        };
        
        Friends.Add(userFriend);
    }
}