using System.IdentityModel.Tokens.Jwt;
using MessagingApp.Api.Common;

namespace MessagingApp.Api.Middleware;

public class JwtParsingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authHeaders = context.Request.Headers.Authorization;
        if (authHeaders.Count > 0)
        {
            var token = context.Request.Headers.Authorization[0]?.Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();

            if (!string.IsNullOrEmpty(token) && handler.CanReadToken(token))
            {
                var jwt = handler.ReadJwtToken(token);
                var userIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
            
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    context.Items[Helpers.UserIdKey] = userId;
                }
            }
        }

        await next(context);
    }
} 