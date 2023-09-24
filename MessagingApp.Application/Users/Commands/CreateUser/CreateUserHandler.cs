using LanguageExt.Common;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IAuthRepository _authRepository;
    public CreateUserHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand req)
    {
        try
        {
            // Suppress warnings as CreateUserDto does not allow null values
            var user = new User(req.Username!, req.Password!);
            var createdUser = await _authRepository.CreateUser(user);

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