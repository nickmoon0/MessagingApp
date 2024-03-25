using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Aggregates;

public class User : IPersistedObject
{
    private const string SpecialSymbols = "!@#$%^&*()[]{}-_=+`~";
    
    public Guid Id { get; private set; }
    public bool Active { get; private set; }
    
    public string? Username { get; private set; }
    public string? HashedPassword { get; private set; }
    public string? Bio { get; set; }

        
    public ICollection<Conversation> Conversations { get; private set; } = [];
    public ICollection<FriendRequest> SentFriendRequests { get; private set; } = [];
    public ICollection<FriendRequest> ReceivedFriendRequests { get; private set; } = [];
    public ICollection<User> Friends { get; private set; } = [];
    
    private User() {}

    private User(string username, string password, string? bio)
    {
        Username = username;
        HashedPassword = password;
        Bio = bio;
        Active = true;
    }

    public static Result<User> CreateNewUser(string username, string password, string? bio, Func<string, string> hashPassword)
    {
        if (!IsUsernameValid(username)) return new FailedToCreateEntityException("Username is not valid");
        if (!IsPasswordValid(password)) return new FailedToCreateEntityException("Password is not valid");

        var hashedPassword = hashPassword(password);
        var user = new User(username, hashedPassword, bio);
        return user;
    }

    public Result<FriendRequest> SendFriendRequest(User receivingUser)
    {
        if (Friends.Contains(receivingUser))
            return new InvalidFriendRequestException("Users are already friends");

        var alreadySent = SentFriendRequests.Any(x => x.SendingUser == this && x.ReceivingUser == receivingUser);
        var alreadyReceived = receivingUser.SentFriendRequests.Any(x => x.SendingUser == receivingUser && x.ReceivingUser == this);
        
        if (alreadySent || alreadyReceived) return new InvalidFriendRequestException("Friend request between users already exists");
        
        var friendRequestResult = FriendRequest.CreateFriendRequest(this, receivingUser);
        if (!friendRequestResult.IsOk) return new InvalidFriendRequestException(friendRequestResult.Error.Message);
        
        var friendRequest = friendRequestResult.Value;
        SentFriendRequests.Add(friendRequest);
        receivingUser.ReceivedFriendRequests.Add(friendRequest);
        
        return friendRequest;
    }

    public Result<FriendRequest> RespondToFriendRequest(
        FriendRequest request, FriendRequestStatus newStatus)
    {
        if (request.ReceivingUser is null || request.SendingUser is null)
            return new InvalidFriendRequestException("Receiving and sending user cannot be null");
        
        // Check that this user was the receiver of the request
        if (!request.ReceivingUser.Equals(this) || !ReceivedFriendRequests.Contains(request))
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

    public Result<User> RemoveFriend(User friendToRemove)
    {
        if (!Friends.Contains(friendToRemove) || !friendToRemove.Friends.Contains(this)) 
            return new FailedToDeleteException("Users are not friends");

        Friends.Remove(friendToRemove);
        friendToRemove.Friends.Remove(this);

        return this;
    }

    public bool LoginUser(string username, Func<string?, bool> comparePasswords)
    {
        var usernameMatches = Username == username;
        var passwordMatches = comparePasswords(HashedPassword);

        return usernameMatches && passwordMatches;
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