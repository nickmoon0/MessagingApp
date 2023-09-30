using MessagingApp.Application.Common.Contracts;
using MessagingApp.Application.Common.Interfaces.Mediator;

namespace MessagingApp.Application.Users.Commands.SendMessage;

public class SendMessageCommand : IRequest<SendMessageResponse>
{
    public string Text { get; set; }
    
    public Guid SendingUserId { get; set; }
    public Guid ReceivingUserId { get; set; }
    public Guid RequestingUserId { get; set; }

    public SendMessageCommand(SendMessageRequest request, Guid requestingUserId)
    {
        Text = request.Text;
        SendingUserId = request.SendingUserId;
        ReceivingUserId = request.ReceivingUserId;
        RequestingUserId = requestingUserId;
    }
}