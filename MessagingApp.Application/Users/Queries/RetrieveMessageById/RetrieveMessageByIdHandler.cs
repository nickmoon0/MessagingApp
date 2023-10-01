using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveMessageById;

public class RetrieveMessageByIdHandler : BaseHandler<RetrieveMessageByIdQuery, GetMessageResponse>
{
    private readonly IUserRepository _userRepository;

    public RetrieveMessageByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    protected override async Task<Result<GetMessageResponse>> HandleRequest(RetrieveMessageByIdQuery request)
    {
        var user = await _userRepository.GetUserById(request.RequestingUserId);  
        if (user == null)
        {
            var ex = new EntityNotFoundException("User does not exist");
            return new Result<GetMessageResponse>(ex);
        }
        var message = await _userRepository.GetMessageById(user.Id, request.MessageId);
        if (message == null)
        {
            var ex = new EntityNotFoundException("Message does not exist");
            return new Result<GetMessageResponse>(ex);
        }

        var response = new GetMessageResponse(message);
        return new Result<GetMessageResponse>(response);
    }
}