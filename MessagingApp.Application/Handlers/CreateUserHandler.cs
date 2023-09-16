using MessagingApp.Application.Commands;
using MessagingApp.Application.Interfaces;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Handlers;

public class CreateUserHandler : IHandler<CreateUserCommand, User>
{
    public User Handle(CreateUserCommand req)
    {
        throw new NotImplementedException();
    }
}