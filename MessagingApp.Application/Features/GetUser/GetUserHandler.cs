using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Features.GetFriends;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.GetUser;

public class GetUserHandler : IHandler<GetUserQuery, GetUserResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetUserHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetUserResponse>> Handle(GetUserQuery request)
    {
        var user = await _applicationContext.Users
            .Include(x => x.Friends)
            .SingleOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) return new FailedToRetrieveEntityException("User does not exist");

        var friends = user.Friends.Select(FriendsResponse.FriendsResponseFromUser);

        return new GetUserResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            Friends = friends
        };
    }
}