namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserRequest
{
    public Guid? Id { get; set; }
    public string? Username { get; set; }
}