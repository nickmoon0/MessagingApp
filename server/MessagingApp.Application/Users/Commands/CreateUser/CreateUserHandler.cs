using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Users.Commands.CreateUser;

public class CreateUserHandler : BaseHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IAuthRepository _authRepository;
    public CreateUserHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    protected override async Task<Result<CreateUserResponse>> HandleRequest(CreateUserCommand request)
    {
        // req.Username/Password cannot be null, CreateUserRequest does not allow null values
        var user = new User 
        {
            Username = request.Username,
            Password = request.Password
        };
            
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
}