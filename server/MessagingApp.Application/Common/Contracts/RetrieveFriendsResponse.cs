namespace MessagingApp.Application.Common.Contracts;

public class RetrieveFriendsResponse
{
    public List<RetrieveUserResponse> Friends { get; set; } = [];
}