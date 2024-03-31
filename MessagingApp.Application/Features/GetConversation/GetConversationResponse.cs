using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Features.GetConversation;

public class GetConversationResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public ConversationType Type { get; init; }
    public required IEnumerable<UserSummaryResponse> Participants { get; init; }
    public required IEnumerable<MessageResponse> Messages { get; init; }
}

public class MessageResponse
{
    public required Guid Id { get; init; }
    public required string? Content { get; init; }
    public required DateTime TimeStamp { get; init; }
    public required UserSummaryResponse SendingUser { get; init; }

    public static MessageResponse MessageResponseFromMessage(Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            Content = message.Content,
            TimeStamp = message.TimeStamp,
            SendingUser = UserSummaryResponse.UserSummaryFromUser(message.SendingUser!)
        };
    }
}

public class UserSummaryResponse
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }

    public static UserSummaryResponse UserSummaryFromUser(User user)
    {
        return new UserSummaryResponse
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}