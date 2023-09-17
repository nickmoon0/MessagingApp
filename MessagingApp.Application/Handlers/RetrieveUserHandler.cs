using FluentValidation;
using LanguageExt.Common;
using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Queries;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Handlers;

public class RetrieveUserHandler : IHandler<RetrieveUserQuery, RetrieveUserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RetrieveUserQuery> _validator;
    public RetrieveUserHandler(IUserRepository userRepository, IValidator<RetrieveUserQuery> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    
    public Result<RetrieveUserDto?> Handle(RetrieveUserQuery req)
    {
        var valResult = _validator.Validate(req);
        if (!valResult.IsValid)
        {
            var valException = new ValidationException(valResult.Errors);
            return new Result<RetrieveUserDto?>(valException);
        }
        
        // Return RetrieveUserDto so that hashed password is never leaked
        RetrieveUserDto? userDto = null;
        
        var user = req.Username == null ? 
            _userRepository.GetUserById(req.Id ?? Guid.Empty) : _userRepository.GetUserByUsername(req.Username);
        
        if (user is not null)
            userDto = new RetrieveUserDto
            {
                Username = user.Username,
                Id = user.Id
            };
        
        return new Result<RetrieveUserDto?>(userDto);
    }
}