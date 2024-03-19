using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.UserFeatures.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly IAuthRepository _authRepository;
    public CreateUserHandler(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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