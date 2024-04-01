using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Application.Features.GetAllConversations;

public class GetAllConversationsResponse
{
    public required IEnumerable<ConversationSummaryResponse> Conversations { get; init; }
}