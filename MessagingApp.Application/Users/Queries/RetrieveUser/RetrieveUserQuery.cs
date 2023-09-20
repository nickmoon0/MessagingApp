using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserQuery : IRequest<RetrieveUserResponse?>
{
    public string? Username { get; set; }
    public Guid? Id { get; set; }

    public RetrieveUserQuery(RetrieveUserDto retrieveUserDto)
    {
        Username = retrieveUserDto.Username;
        Id = retrieveUserDto.Id;
    }
}