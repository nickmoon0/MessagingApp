﻿using LanguageExt.Common;
using MessagingApp.Application.Common.BaseClasses;
using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Mediator;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Users.Commands.SendMessage;

public class SendMessageHandler : BaseHandler<SendMessageCommand, SendMessageResponse>
{
    private readonly IUserRepository _userRepository;
    
    public SendMessageHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    protected override async Task<Result<SendMessageResponse>> HandleRequest(SendMessageCommand request)
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

        var createdMessage = sendingUser.SendMessage(message, request.RequestingUserId);
        await _userRepository.UpdateUser(sendingUser);

        var response = new SendMessageResponse(createdMessage.Id, createdMessage.Text, createdMessage.Timestamp);
        return new Result<SendMessageResponse>(response);
    }
}