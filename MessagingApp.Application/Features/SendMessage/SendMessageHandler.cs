using MessagingApp.Application.Common;
using MessagingApp.Application.Common.Contexts;
using MessagingApp.Application.Common.ResponseEntities;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Application.Features.SendMessage;

public class SendMessageHandler : IHandler<SendMessageCommand, SendMessageResponse>
{
    private readonly IApplicationContext _applicationContext;

    public SendMessageHandler(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<Result<SendMessageResponse>> Handle(SendMessageCommand request)
    {
        var user = await _applicationContext.Users.FindAsync(request.SendingUserId);
        var conversation = await _applicationContext.Conversations
            .Include(x => x.Participants)
            .SingleOrDefaultAsync(x => x.Id == request.ConversationId);

        if (user == null) return new FailedToRetrieveEntityException("User does not exist");
        if (conversation == null) return new FailedToRetrieveEntityException("Conversation does not exist");

        var messageResult = conversation.SendMessage(user, request.Content);
        if (!messageResult.IsOk) return messageResult.Error;

        var message = messageResult.Value;
        await _applicationContext.Messages.AddAsync(message);
        await _applicationContext.SaveChangesAsync();

        var messageResponse = MessageResponse.FromMessage(message);
        return new SendMessageResponse { Message = messageResponse };
    }
}