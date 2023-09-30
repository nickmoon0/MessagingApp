using LanguageExt.Common;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.SendMessage;

public class SendMessageHandler : IHandler<SendMessageCommand, SendMessageResponse>
{
    private readonly IUserRepository _userRepository;
    
    public SendMessageHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<SendMessageResponse>> Handle(SendMessageCommand req)
    {
        try
        {
            var message = new Message()
            {
                ReceivingUserId = req.ReceivingUserId,
                SendingUserId = req.SendingUserId,
                Text = req.Text,
                Timestamp = DateTime.Now
            };

            var sendingUser = await _userRepository.GetUserById(req.SendingUserId);
            var receivingUser = await _userRepository.GetUserById(req.ReceivingUserId);

            if (sendingUser == null || receivingUser == null)
            {
                throw new EntityNotFoundException("User could not be found when sending message");
            }

            sendingUser.SendMessage(message, req.RequestingUserId);
            await _userRepository.UpdateUser(sendingUser);
            
            return new Result<SendMessageResponse>(new SendMessageResponse());
        }
        catch (InvalidOperationException ex)
        {
            return new Result<SendMessageResponse>(ex);
        }
        catch (Exception ex)
        {
            return new Result<SendMessageResponse>(ex);
        }
    }
}