using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserHandler : IHandler<RetrieveUserQuery, RetrieveUserResponse?>
{
    private readonly IAuthRepository _authRepository;
    private readonly IValidator<RetrieveUserQuery> _validator;
    public RetrieveUserHandler(IAuthRepository authRepository, IValidator<RetrieveUserQuery> validator)
    {
        _authRepository = authRepository;
        _validator = validator;
    }
    
    public async Task<Result<RetrieveUserResponse?>> Handle(RetrieveUserQuery req)
    {
        var valResult = await _validator.ValidateAsync(req);
        if (!valResult.IsValid)
        {
            var valException = new ValidationException(valResult.Errors);
            return new Result<RetrieveUserResponse?>(valException);
        }
        
        // Return RetrieveUserDto so that hashed password is never leaked
        RetrieveUserResponse? userResponse = null;
        
        // Suppress warning as validator ensures these are not null
        var user = req.Username == null ?
            await _authRepository.GetUserById((Guid)req.Id!) : await _authRepository.GetUserByUsername(req.Username);
        
        if (user is not null)
            userResponse = new RetrieveUserResponse
            {
                Username = user.Username!, // Username will always be populated if user != null
                Id = user.Id
            };
        
        return new Result<RetrieveUserResponse?>(userResponse);
    }
}