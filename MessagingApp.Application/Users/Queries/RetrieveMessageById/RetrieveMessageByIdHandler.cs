using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.RetrieveMessageById;

public class RetrieveMessageByIdHandler : IHandler<RetrieveMessageByIdQuery, GetMessageResponse>
{
    private readonly IUserRepository _userRepository;

    public RetrieveMessageByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetMessageResponse>> Handle(RetrieveMessageByIdQuery req)
    {
        try
        {
            var user = await _userRepository.GetUserById(req.RequestingUserId);  
            if (user == null)
            {
                var ex = new EntityNotFoundException("User does not exist");
                return new Result<GetMessageResponse>(ex);
            }
            var message = user.GetMessageById(req.MessageId);
            if (message == null)
            {
                var ex = new EntityNotFoundException("Message does not exist");
                return new Result<GetMessageResponse>(ex);
            }

            var response = new GetMessageResponse(message);
            return new Result<GetMessageResponse>(response);
        }
        catch (Exception ex)
        {
            return new Result<GetMessageResponse>(ex);
        }
    }
}