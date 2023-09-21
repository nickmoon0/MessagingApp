namespace MessagingApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    
    public User() {}

    public User(string? username)
    {
        Username = username;
    }
}