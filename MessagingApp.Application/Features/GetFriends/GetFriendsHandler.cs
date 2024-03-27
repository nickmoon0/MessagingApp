using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.GetFriends;

public class GetFriendsHandler : IHandler<GetFriendsQuery, GetFriendsResponse>
{
    private readonly IApplicationContext _applicationContext;

    public GetFriendsHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<GetFriendsResponse>> Handle(GetFriendsQuery request)
    {
        var user = await _applicationContext.Users
            .Include(x => x.Friends)
            .SingleOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) return new FailedToRetrieveEntityException("User does not exist");

        var friends = user.Friends.Select(FriendsResponse.FriendsResponseFromUser);

        return new GetFriendsResponse { Friends = friends };
    }
}