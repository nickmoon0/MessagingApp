using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommand> _validator;
    public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    public async Task<Result<Guid>> Handle(CreateUserCommand req)
    {
        try
        {
            var result = await _validator.ValidateAsync(req);
            if (!result.IsValid)
            {
                var valException = new ValidationException(result.Errors);
                return new Result<Guid>(valException);
            }

            // Suppress warnings as validator ensures these values are not null
            var user = new User(req.Username!, req.Password!);
            var createdUser = await _userRepository.CreateUser(user);
            
            if (createdUser != null) return new Result<Guid>(createdUser.Id);
            
            var ex = new Exception("Could not create user");
            return new Result<Guid>(ex);

        }
        catch (EntityAlreadyExistsException ex)
        {
            return new Result<Guid>(ex);
        }
    }
}