namespace MessagingApp.Application.Common.Contracts;

public class RetrieveUserRequest
{
    public Guid? Id { get; set; }
    public string? Username { get; set; }
}