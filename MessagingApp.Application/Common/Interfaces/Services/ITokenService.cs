using MessagingApp.Application.Common.DTOs;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Common.Interfaces.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}