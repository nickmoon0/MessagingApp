using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.UserFeatures.RetrieveFriends;

public class RetrieveFriendsQuery : IRequest<Result<RetrieveFriendsResponse>>
{
    public Guid UserId { get; set; }
}