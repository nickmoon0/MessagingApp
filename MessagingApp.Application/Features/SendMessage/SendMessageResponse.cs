using MessagingApp.Application.Common.ResponseEntities;

namespace MessagingApp.Application.Features.SendMessage;

public class SendMessageResponse
{
    public required MessageResponse Message { get; init; }
}