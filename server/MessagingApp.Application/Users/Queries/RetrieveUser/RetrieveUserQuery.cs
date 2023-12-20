using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.Users.Queries.RetrieveUser;

public class RetrieveUserQuery : IRequest<Result<RetrieveUserResponse?>>
{
    public string? Username { get; set; }
    public Guid? Id { get; set; }

    public RetrieveUserQuery(RetrieveUserRequest retrieveUserRequest)
    {
        Username = retrieveUserRequest.Username;
        Id = retrieveUserRequest.Id;
    }
}