using MessagingApp.Application.Common.ResponseEntities;

namespace MessagingApp.Application.Features.CreateConversation;

public class CreateConversationResponse
{
    public required ConversationSummaryResponse Conversation { get; init; }
    public required IEnumerable<UserSummaryResponse> Participants { get; init; }
}