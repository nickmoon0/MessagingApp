namespace MessagingApp.Infrastructure.Data.Models;

public class RequestStatus
{
    public RequestStatuses Id { get; set; }
    public string Name { get; set; } = null!;
}

public enum RequestStatuses
{
    Pending,
    Accepted,
    Declined
}