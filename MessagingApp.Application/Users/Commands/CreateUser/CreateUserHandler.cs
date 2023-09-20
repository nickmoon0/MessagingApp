using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserCommand> _validator;
    public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand req)
    {
        try
        {
            // Ensure all required values are present
            var result = await _validator.ValidateAsync(req);
            if (!result.IsValid)
            {
                var valException = new ValidationException(result.Errors);
                return new Result<CreateUserResponse>(valException);
            }

            // Suppress warnings as validator ensures these values are not null
            var user = new User(req.Username!, req.Password!);
            var createdUser = await _userRepository.CreateUser(user);

            // Not null if created successfully
            if (createdUser != null)
            {
                // createdUser.Username will always be populated if createdUser != null
                var userResponse = new CreateUserResponse { Id = createdUser.Id, Username = createdUser.Username! };
                return new Result<CreateUserResponse>(userResponse);
            }

            var ex = new CouldNotCreateEntityException("Could not create user");
            return new Result<CreateUserResponse>(ex);
        }
        catch (BadValuesException ex)
        {
            return new Result<CreateUserResponse>(ex);
        }
        catch (EntityAlreadyExistsException ex)
        {
            return new Result<CreateUserResponse>(ex);
        }
    }
}