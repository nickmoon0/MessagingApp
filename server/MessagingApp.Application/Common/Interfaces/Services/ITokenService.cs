using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Common.Interfaces.Services;

public interface ITokenService
{
    /// <summary>
    /// Generates a JWT once a user has been authenticated
    /// </summary>
    /// <param name="user"></param>
    /// <returns>String JWT</returns>
    public string GenerateToken(User user);
}