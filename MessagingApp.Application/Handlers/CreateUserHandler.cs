using MessagingApp.Application.Commands;
using MessagingApp.Application.Interfaces;
using MessagingApp.Application.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Handlers;

public class CreateUserHandler : IHandler<CreateUserCommand, Guid>
{
    private IUserRepository _userRepository;
    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Guid Handle(CreateUserCommand req)
    {
        var user = new User(req.Username, req.Password);
        _userRepository.CreateUser(user);
        return user.Id;
    }
}