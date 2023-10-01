using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Common;

namespace MessagingApp.Application.Users.Queries.RetrievePendingFriendRequests;

public class RetrievePendingFriendRequestHandler : 
    BaseHandler<RetrievePendingFriendRequestsQuery, RetrievePendingFriendRequestsResponse>
{
    private readonly IUserRepository _userRepository;

    public RetrievePendingFriendRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<Result<RetrievePendingFriendRequestsResponse>> HandleRequest(
        RetrievePendingFriendRequestsQuery request)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        if (user == null) throw new EntityNotFoundException("User does not exist");

        var friendRequests = await _userRepository.GetUsersFriendRequests(user.Id, FriendRequestStatus.Pending);
        var response = new RetrievePendingFriendRequestsResponse(friendRequests);

        return response;
    }
}