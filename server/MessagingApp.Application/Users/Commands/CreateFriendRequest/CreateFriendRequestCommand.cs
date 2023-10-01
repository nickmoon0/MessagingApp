using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Commands.CreateFriendRequest;

public class CreateFriendRequestCommand : IRequest<CreateFriendRequestResponse>
{
    public Guid FromUser { get; set; }
    public Guid ToUser { get; set; }

    public Guid RequestingUser { get; set; }

    public CreateFriendRequestCommand(Guid toUserId, Guid requestingUser)
    {
        FromUser = requestingUser;
        ToUser = toUserId;
        RequestingUser = requestingUser;
    }
}