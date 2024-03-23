using MessagingApp.Application.Models;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Common;

namespace MessagingApp.Application.Common.Services;

public interface ITokenService
{
    public Task<Result<TokenSet, Exception>> RotateTokens(User user);
    public Guid ExtractUserIdFromAccessToken(string token);
}