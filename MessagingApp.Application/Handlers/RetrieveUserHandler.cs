using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Application.Queries;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Handlers;

public class RetrieveUserHandler : IHandler<RetrieveUserQuery, RetrieveUserDto?>
{
    private readonly IUserRepository _userRepository;
    public RetrieveUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public RetrieveUserDto? Handle(RetrieveUserQuery req)
    {
        User? user;
        // Return RetrieveUserDto so that hashed password is never leaked
        RetrieveUserDto? userDto = null;
        
        if (req.Username is not null)
            user = _userRepository.GetUserByUsername(req.Username);
        else if (req.Id is not null)
            user = _userRepository.GetUserById((Guid)req.Id);
        else
            throw new Exception(); // TODO: Create proper exception

        if (user is not null)
            userDto = new RetrieveUserDto
            {
                Username = user.Username,
                Id = user.Id
            };
        
        return userDto;
    }
}