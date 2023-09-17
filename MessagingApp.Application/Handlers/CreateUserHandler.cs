using LanguageExt.Common;
using MessagingApp.Application.Commands;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Handlers;

public class CreateUserHandler : IHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Result<Guid> Handle(CreateUserCommand req)
    {
        try
        {
            var user = new User(req.Username, req.Password);
            _userRepository.CreateUser(user);
            return new Result<Guid>(user.Id);
        }
        catch (EntityAlreadyExistsException ex)
        {
            return new Result<Guid>(ex);
        }
    }
}