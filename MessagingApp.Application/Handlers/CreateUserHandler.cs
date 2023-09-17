using System.Diagnostics;
using FluentValidation;
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
    private readonly IValidator<CreateUserCommand> _validator;
    public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    public Result<Guid> Handle(CreateUserCommand req)
    {
        try
        {
            var result = _validator.Validate(req);
            if (!result.IsValid)
            {
                var valException = new ValidationException(result.Errors);
                return new Result<Guid>(valException);
            }

            // Suppress warnings as validator ensures these values are not null
            var user = new User(req.Username!, req.Password!);
            _userRepository.CreateUser(user);
            return new Result<Guid>(user.Id);
        }
        catch (EntityAlreadyExistsException ex)
        {
            return new Result<Guid>(ex);
        }
    }
}