using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using MessagingApp.Domain.Common.Exceptions;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Common;
using MessagingApp.Application.Features.GetUserByName;

namespace MessagingApp.Application.Features.GetUserByName;

public class GetUserByNameHandler : IHandler<GetUserByNameQuery, GetUserByNameResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetUserByNameHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetUserByNameResponse>> Handle(GetUserByNameQuery request)
    {
        var user = await _applicationContext.Users
            .Include(u => u.Friends)
            .SingleOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());


        if (user == null) return new FailedToRetrieveEntityException("Username does not exist");

        var friends = user.Friends.Select(UserSummaryResponse.FromUser);

        return new GetUserByNameResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            Friends = friends
        };
    }
}


