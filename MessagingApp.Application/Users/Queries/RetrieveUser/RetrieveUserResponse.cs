namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserResponse
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
}