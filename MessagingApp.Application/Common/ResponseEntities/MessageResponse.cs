using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.ResponseEntities;

public class MessageResponse
{
    public required Guid Id { get; init; }
    public required string? Content { get; init; }
    public required DateTime TimeStamp { get; init; }
    public required UserSummaryResponse SendingUser { get; init; }

    public static MessageResponse FromMessage(Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            Content = message.Content,
            TimeStamp = message.TimeStamp,
            SendingUser = UserSummaryResponse.FromUser(message.SendingUser!)
        };
    }
}