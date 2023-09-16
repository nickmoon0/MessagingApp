using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces;
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