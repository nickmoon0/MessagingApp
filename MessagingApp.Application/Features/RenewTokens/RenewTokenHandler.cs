using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.Services;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.RenewTokens;

public class RenewTokenHandler : IHandler<RenewTokenCommand, RenewTokenResponse>
{
    private readonly ITokenContext _tokenContext;
    private readonly ITokenService _tokenService;

    public RenewTokenHandler(ITokenContext tokenContext, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _tokenContext = tokenContext;
    }

    public async Task<Result<RenewTokenResponse>> Handle(RenewTokenCommand request)
    {
        var refreshToken = await _tokenContext.RefreshTokens
            .Include(x => x.Owner)
            .SingleOrDefaultAsync(x => x.Token == request.RefreshToken && x.Active);
        
        if (refreshToken == null) return new FailedToRetrieveEntityException("Invalid refresh token");
        if (!refreshToken.Active) return new InvalidCredentialsException("Refresh token is no longer valid");
        
        // If tokens expired it should be inactivated and user should reauthenticate 
        if (DateTime.UtcNow >= refreshToken.ExpiryDate)
        {
            refreshToken.InactivateToken();
            await _tokenContext.SaveChangesAsync();
            return new InvalidCredentialsException("Refresh token has expired");
        }
        
        var newTokensResult = await _tokenService.RotateTokens(refreshToken.Owner!);
        if (!newTokensResult.IsOk) return new Exception("Failed to generate new token set");
        
        var newTokens = newTokensResult.Value;
        return new RenewTokenResponse { Tokens = newTokens };
    }
}