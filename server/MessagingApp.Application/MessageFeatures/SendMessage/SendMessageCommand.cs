using LanguageExt.Common;
using MediatR;
using MessagingApp.Application.Common.Contracts;

namespace MessagingApp.Application.MessageFeatures.SendMessage;

public class SendMessageCommand : IRequest<Result<SendMessageResponse>>
{
    public string Text { get; set; }
    
    public Guid SendingUserId { get; set; }
    public Guid ReceivingUserId { get; set; }
    public Guid RequestingUserId { get; set; }

    public SendMessageCommand(SendMessageRequest request, Guid receivingUserId, Guid requestingUserId)
    {
        Text = request.Text;
        SendingUserId = requestingUserId;
        ReceivingUserId = receivingUserId;
        RequestingUserId = requestingUserId;
    }
}