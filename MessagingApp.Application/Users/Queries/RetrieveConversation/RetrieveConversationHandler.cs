using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveConversation;

public class RetrieveConversationHandler : IHandler<RetrieveConversationQuery, RetrieveConversationResponse>
{
    private readonly IUserRepository _userRepository;

    public RetrieveConversationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RetrieveConversationResponse>> Handle(RetrieveConversationQuery req)
    {
        try
        {
            var messages = await _userRepository.GetConversation(
                req.RequestingUserId, req.UserId);

            var response = new RetrieveConversationResponse(
                req.RequestingUserId, req.UserId, messages);

            return new Result<RetrieveConversationResponse>(response);
        }
        catch (Exception ex)
        {
            return new Result<RetrieveConversationResponse>(ex);
        }
    }
}