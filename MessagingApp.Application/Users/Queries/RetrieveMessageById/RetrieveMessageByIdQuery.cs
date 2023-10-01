using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.RetrieveMessageById;

public class RetrieveMessageByIdQuery : IRequest<GetMessageResponse>
{
    public Guid MessageId { get; set; }
    public Guid RequestingUserId { get; set; }

    public RetrieveMessageByIdQuery(Guid messageId, Guid requestingUser)
    {
        MessageId = messageId;
        RequestingUserId = requestingUser;
    }
}