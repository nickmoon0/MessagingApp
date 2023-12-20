using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Helpers;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.SendMessage;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, Result<SendMessageResponse>>
{
    private readonly IUserRepository _userRepository;
    
    public SendMessageHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<SendMessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            ReceivingUserId = request.ReceivingUserId,
            SendingUserId = request.SendingUserId,
            Text = request.Text,
            Timestamp = DateTime.Now
        };

        var sendingUser = await _userRepository.GetUserById(request.SendingUserId);
        var receivingUser = await _userRepository.GetUserById(request.ReceivingUserId);

        if (sendingUser == null || receivingUser == null)
        {
            throw new EntityNotFoundException("User could not be found when sending message");
        }
        
        var result = sendingUser.SendMessage(message, request.RequestingUserId);
        if (!result.Success)
            return new Result<SendMessageResponse>(ExceptionHelper.ResolveException(result.Error!));
        
        // Result wont be nul if operation was a success
        var createdMessage = result.Result!;
        await _userRepository.UpdateUser(sendingUser);

        var response = new SendMessageResponse(createdMessage.Id, createdMessage.Text, createdMessage.Timestamp);
        return new Result<SendMessageResponse>(response);
    }
}