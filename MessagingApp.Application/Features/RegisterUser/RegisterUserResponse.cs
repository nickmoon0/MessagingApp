using MessagingApp.Application.Models;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.RegisterUser;

public class RegisterUserResponse
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required TokenSet Tokens { get; init; }
    
    public string? Bio { get; init; }

}