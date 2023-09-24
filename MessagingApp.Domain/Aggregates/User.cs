using MessagingApp.Domain.Exceptions;
using MessagingApp.Domain.Validators;

namespace MessagingApp.Domain.Aggregates;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }

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