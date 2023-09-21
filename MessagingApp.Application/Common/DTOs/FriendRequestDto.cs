namespace MessagingApp.Application.Common.DTOs;

public class FriendRequestDto
{
    public Guid ToUser { get; set; }
    public Guid FromUser { get; set; }

    public FriendRequestDtoStatus Status { get; set; }
}

public enum FriendRequestDtoStatus
{
    Pending,
    Accepted,
    Declined
}