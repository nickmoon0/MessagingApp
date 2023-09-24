using MessagingApp.Domain.Exceptions;
using MessagingApp.Domain.Validators;

namespace MessagingApp.Domain.Aggregates;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }

    private string _password;
    public string? Password
    {
        set
        {
            if (value == null) throw new InvalidOperationException();
            _password = BCrypt.Net.BCrypt.HashPassword(value);
        }
    }
    public string? HashedPassword => _password;

    public List<User> Friends { get; set; }
    public List<FriendRequest> SentFriendRequests { get; set; }
    public List<FriendRequest> ReceivedFriendRequests { get; set; }

    public User(Guid id)
    {
        Id = id;
        Friends = new List<User>();
        SentFriendRequests = new List<FriendRequest>();
        ReceivedFriendRequests = new List<FriendRequest>();
    }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    
    public User(Guid id, string? username)
    {
        Id = id;
        Username = username;
        Friends = new List<User>();
        SentFriendRequests = new List<FriendRequest>();
        ReceivedFriendRequests = new List<FriendRequest>();
    }

    public void SendFriendRequest(FriendRequest request, Guid requestingUser)
    {
        var validator = new FriendRequestValidator(requestingUser);
        var valResult = validator.Validate(request);
        if (!valResult.IsValid)
        {
            throw new InvalidOperationException();
        }
        
        SentFriendRequests.Add(request);
    }

    public void AcceptFriendRequest(FriendRequest request, Guid requestingUser)
    {
        throw new NotImplementedException();
    }
}