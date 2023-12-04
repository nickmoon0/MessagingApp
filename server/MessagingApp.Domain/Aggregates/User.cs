using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;
using MessagingApp.Domain.Exceptions;
using MessagingApp.Domain.Services;
using MessagingApp.Domain.Validators;

namespace MessagingApp.Domain.Aggregates;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }

    public string? Password { get; set; }

    public HashSet<UserFriend> Friends { get; set; } = new();
    public List<FriendRequest> SentFriendRequests { get; set; } = new();
    public List<FriendRequest> ReceivedFriendRequests { get; set; } = new();

    public List<Message> SentMessages { get; set; } = new();
    public List<Message> ReceivedMessages { get; set; } = new();
    
    /// <summary>
    /// Sends a friend request to a given user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="requestingUser">The user who created the request</param>
    /// <exception cref="DomainException">Thrown if validation fails</exception>
    public void SendFriendRequest(FriendRequest request, Guid requestingUser)
    {
        // Ensures the request is valid
        var validator = new SendFriendRequestValidator(
            requestingUser, SentFriendRequests, ReceivedFriendRequests, Friends);
        var valResult = validator.Validate(request);
        
        if (!valResult.IsValid)
        {
            throw ValidationErrorService.GetException(valResult);
        }
        
        SentFriendRequests.Add(request);
    }

    /// <summary>
    /// Accepts a friend request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="requestingUser">User who requested that the request be accepted</param>
    /// <exception cref="DomainException">Thrown if validation fails</exception>
    /// <exception cref="InternalServerErrorException">Thrown if user fails to be added to friends list</exception>
    public void AcceptFriendRequest(FriendRequest request, Guid requestingUser)
    {
        var validator = new AcceptFriendRequestValidator(requestingUser, ReceivedFriendRequests);
        var valResult = validator.Validate(request);

        if (!valResult.IsValid)
        {
            throw ValidationErrorService.GetException(valResult);
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

        if (!Friends.Add(userFriend))
        {
            throw new InternalServerErrorException("Failed to add friend when accepting friend request", 
                ErrorCodes.InternalServerError);
        }
    }

    /// <summary>
    /// Adds a friend when a friend request has been accepted
    /// </summary>
    /// <param name="userToAdd"></param>
    /// <exception cref="InternalServerErrorException">Thrown if user fails to be added to friends list</exception>
    public void AddFriend(Guid userToAdd)
    {
        var userFriend = new UserFriend
        {
            UserId = Id,
            FriendId = userToAdd
        };

        if (!Friends.Add(userFriend))
        {
            throw new InternalServerErrorException("Failed to add friend", ErrorCodes.InternalServerError);
        }
    }

    /// <summary>
    /// Sends a message to a given user
    /// </summary>
    /// <param name="message"></param>
    /// <param name="requestingUser"></param>
    /// <returns></returns>
    /// <exception cref="DomainException">Thrown if validation fails</exception>
    public Message SendMessage(Message message, Guid requestingUser)
    {
        var validator = new SendMessageValidator(requestingUser, Friends);
        var valResult = validator.Validate(message);

        if (!valResult.IsValid)
        {
            throw ValidationErrorService.GetException(valResult);
        }
        
        SentMessages.Add(message);
        return message;
    }
}