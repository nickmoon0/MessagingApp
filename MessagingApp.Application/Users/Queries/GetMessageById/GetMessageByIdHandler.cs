using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;

namespace MessagingApp.Application.Users.Queries.GetMessageById;

public class GetMessageByIdHandler : IHandler<GetMessageByIdQuery, GetMessageResponse>
{
    private readonly IUserRepository _userRepository;

    public GetMessageByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetMessageResponse>> Handle(GetMessageByIdQuery req)
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