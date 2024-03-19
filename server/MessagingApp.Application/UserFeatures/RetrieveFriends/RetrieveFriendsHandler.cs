using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.UserFeatures.RetrieveFriends;

public class RetrieveFriendsHandler : IRequestHandler<RetrieveFriendsQuery, Result<RetrieveFriendsResponse>>
{
    private readonly IUserRepository _userRepository;

    public RetrieveFriendsHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RetrieveFriendsResponse>> Handle(RetrieveFriendsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            var ex = new EntityNotFoundException($"User {request.UserId} could not be found");
            return new Result<RetrieveFriendsResponse>(ex);
        }

        var friends = await _userRepository.GetUsersFriends(user.Id);
        
        var response = new RetrieveFriendsResponse
        {
            Friends = friends.Select(x => new RetrieveUserResponse
            {
                Id = x.Id,
                Username = x.Username!
            }).ToList()
        };

        return new Result<RetrieveFriendsResponse>(response);
    }
}