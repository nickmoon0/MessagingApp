namespace MessagingApp.Application.Common.Contracts;

public class CreateUserResponse
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
}