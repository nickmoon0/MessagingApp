using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Services;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserHandler : IHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IAuthService _authService;
    public CreateUserHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand req)
    {
        try
        {
            // req.Username/Password cannot be null, CreateUserRequest does not allow null values
            var user = new User 
            {
                Username = req.Username,
                Password = req.Password
            };
            
            var createdUser = await _authService.CreateUser(user);

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