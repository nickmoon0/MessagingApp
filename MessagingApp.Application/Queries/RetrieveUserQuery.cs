using MessagingApp.Application.Common.DTOs;
using MessagingApp.Application.Common.Interfaces;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Queries;

public class RetrieveUserQuery : IRequest<RetrieveUserDto?>
{
    public string? Username { get; set; }
    public Guid? Id { get; set; }

    public RetrieveUserQuery(RetrieveUserDto retrieveUserDto)
    {
        Username = retrieveUserDto.Username;
        Id = retrieveUserDto.Id;
    }
}