using MessagingApp.Application.Common.DTOs;

namespace MessagingApp.Application.Common.Interfaces.Services;

public interface ITokenService
{
    public string GenerateToken(UserDto user);
}