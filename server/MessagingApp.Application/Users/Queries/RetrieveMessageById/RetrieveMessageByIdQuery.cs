using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.Users.Queries.RetrieveMessageById;

public class RetrieveMessageByIdQuery : IRequest<Result<GetMessageResponse>>
{
    public Guid MessageId { get; set; }
    public Guid RequestingUserId { get; set; }

    public RetrieveMessageByIdQuery(Guid messageId, Guid requestingUser)
    {
        MessageId = messageId;
        RequestingUserId = requestingUser;
    }
}