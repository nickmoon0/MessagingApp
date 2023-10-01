using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserQuery : IRequest<RetrieveUserResponse?>
{
    public string? Username { get; set; }
    public Guid? Id { get; set; }

    public RetrieveUserQuery(RetrieveUserRequest retrieveUserRequest)
    {
        Username = retrieveUserRequest.Username;
        Id = retrieveUserRequest.Id;
    }
}