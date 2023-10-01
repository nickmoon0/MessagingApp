namespace MessagingApp.Application.Common.Contracts;

public class CreateFriendRequestResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public CreateFriendRequestResponse(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
}