using MessagingApp.Application.DTOs;
using MessagingApp.Application.Interfaces;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Commands;

public class CreateUserCommand : IRequest<Guid>
{
    public string Username { get; init; }
    public string Password { get; init; }
    
    public CreateUserCommand(CreateUserDto createUserDto)
    {
        Username = createUserDto.Username;
        Password = createUserDto.Password;
    }
}