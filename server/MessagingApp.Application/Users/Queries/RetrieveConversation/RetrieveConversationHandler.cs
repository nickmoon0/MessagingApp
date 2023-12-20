using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveConversation;

public class RetrieveConversationHandler : IRequestHandler<RetrieveConversationQuery, Result<RetrieveConversationResponse>>
{
    private readonly IUserRepository _userRepository;

    public RetrieveConversationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RetrieveConversationResponse>> Handle(RetrieveConversationQuery request, CancellationToken cancellationToken)
    {
        var messages = await _userRepository.GetConversation(
            request.RequestingUserId, request.UserId);

        var response = new RetrieveConversationResponse(
            request.RequestingUserId, request.UserId, messages);

        return new Result<RetrieveConversationResponse>(response);
    }
}