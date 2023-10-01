using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveConversation;

public class RetrieveConversationHandler : BaseHandler<RetrieveConversationQuery, RetrieveConversationResponse>
{
    private readonly IUserRepository _userRepository;

    public RetrieveConversationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<Result<RetrieveConversationResponse>> HandleRequest(RetrieveConversationQuery request)
    {
        var messages = await _userRepository.GetConversation(
            request.RequestingUserId, request.UserId);

        var response = new RetrieveConversationResponse(
            request.RequestingUserId, request.UserId, messages);

        return new Result<RetrieveConversationResponse>(response);
    }
}