using FluentValidation;
using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.UserFeatures.RetrieveUser;

public class RetrieveUserHandler : IRequestHandler<RetrieveUserQuery, Result<RetrieveUserResponse?>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IValidator<RetrieveUserQuery> _validator;
    public RetrieveUserHandler(IUserRepository userRepository, IValidator<RetrieveUserQuery> validator)
    {
        _validator = validator;
        _userRepository = userRepository;
    }
    
    public async Task<Result<RetrieveUserResponse?>> Handle(RetrieveUserQuery request, CancellationToken cancellationToken)
    {
        var valResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!valResult.IsValid)
        {
            var valException = new ValidationException(valResult.Errors);
            return new Result<RetrieveUserResponse?>(valException);
        }
        
        // Return RetrieveUserDto so that hashed password is never leaked
        RetrieveUserResponse? userResponse = null;
        
        // Suppress warning as validator ensures these are not null
        var user = request.Username == null ?
            await _userRepository.GetUserById((Guid)request.Id!, false) : 
            await _userRepository.GetUserByUsername(request.Username, false);
        
        if (user is not null)
            userResponse = new RetrieveUserResponse
            {
                Username = user.Username!, // Username will always be populated if user != null
                Id = user.Id
            };
        
        return new Result<RetrieveUserResponse?>(userResponse);
    }
}