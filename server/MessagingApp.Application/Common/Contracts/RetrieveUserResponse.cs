namespace MessagingApp.Application.Common.Contracts;

public class RetrieveUserResponse
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
}