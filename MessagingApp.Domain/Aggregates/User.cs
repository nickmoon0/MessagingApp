using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class User : IDomainObject
{
    private const string SpecialSymbols = "!@#$%^&*()[]{}-_=+`~";
    
    public Guid Id { get; private set; }
    public bool Active { get; private set; }
    
    public string? Username { get; private set; }
    public string? HashedPassword { get; private set; }
    public string? Bio { get; set; }

    private User() {}

    private User(string username, string password, string? bio)
    {
        Username = username;
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        Bio = bio;
        Active = true;
    }
    
    public ICollection<Conversation> Conversations { get; private set; } = [];
    public ICollection<FriendRequest> FriendRequests { get; private set; } = [];
    public ICollection<User> Friends { get; private set; } = [];

    public static Result<User, InvalidUserException> CreateNewUser(string username, string password, string? bio = null)
    {
        if (!IsUsernameValid(username)) return new InvalidUserException("Username is not valid");
        if (!IsPasswordValid(password)) return new InvalidUserException("Password is not valid");

        var user = new User(username, password, bio);
        return user;
    }

    public Result<FriendRequest, InvalidFriendRequestException> SendFriendRequest(User receivingUser)
    {
        if (this == receivingUser)
            return new InvalidFriendRequestException("User cannot send a friend request to themself");
        if (Friends.Contains(receivingUser))
            return new InvalidFriendRequestException("Users are already friends");

        var friendRequestResult = FriendRequest.CreateFriendRequest(this, receivingUser, FriendRequestStatus.Pending);
        var friendRequest = friendRequestResult.Value; // Will never fail/error
        
        receivingUser.FriendRequests.Add(friendRequest);
        FriendRequests.Add(friendRequest);
        
        return friendRequest;
    }

    public Result<FriendRequest, InvalidFriendRequestException> RespondToFriendRequest(
        FriendRequest request, FriendRequestStatus newStatus)
    {
        // Check that this user was the receiver of the request
        if (!request.ReceivingUser.Equals(this) || !FriendRequests.Contains(request))
            return new InvalidFriendRequestException("User did not receive friend request");
        // Check that request is still pending
        if (request.Status != FriendRequestStatus.Pending)
            return new InvalidFriendRequestException("User has already responded to request");
        if (newStatus == FriendRequestStatus.Pending)
            return new InvalidFriendRequestException("New status is not valid");
        
        request.Status = newStatus;

        if (newStatus == FriendRequestStatus.Rejected) return request;

        // Friend request accepted
        var sendingUser = request.SendingUser;
        
        Friends.Add(request.SendingUser);
        sendingUser.Friends.Add(this);

        return request;
    }
    
    // Helper methods
    private static bool IsUsernameValid(string username)
    {
        var valid = 
            username.Length > 6 &&
            !username.Any(x => SpecialSymbols.Contains(x));
        return valid;
    }
    private static bool IsPasswordValid(string password)
    {
        var valid = 
            password.Length > 8 &&
            password.Any(char.IsUpper) &&
            password.Any(char.IsDigit) &&
            password.Any(x => SpecialSymbols.Contains(x));
        
        return valid;
    }
}