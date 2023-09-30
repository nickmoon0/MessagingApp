using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.GetMessageById;

public class GetMessageByIdQuery : IRequest<GetMessageResponse>
{
    public Guid MessageId { get; set; }
    public Guid RequestingUserId { get; set; }

    public GetMessageByIdQuery(Guid messageId, Guid requestingUser)
    {
        MessageId = messageId;
        RequestingUserId = requestingUser;
    }
}