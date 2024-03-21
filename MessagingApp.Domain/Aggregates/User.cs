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
    public IEnumerable<FriendRequest> FriendRequests { get; set; } = [];
    public IEnumerable<Conversation> Conversations { get; set; } = [];

    public static Result<User, InvalidUserException> CreateNewUser(string username, string password, string? bio = null)
    {
        if (!IsUsernameValid(username)) return new InvalidUserException("Username is not valid");
        if (!IsPasswordValid(password)) return new InvalidUserException("Password is not valid");

        var user = new User(username, password, bio);
        return user;
    }
    
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