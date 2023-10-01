using FluentValidation;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

public class SendMessageValidator : AbstractValidator<Message>
{
    public SendMessageValidator(
        Guid requestingUser,
        IReadOnlyCollection<UserFriend> friends)
    {
        RuleFor(x => x.Text)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Receiving user cannot be null or empty");
        
        RuleFor(x => x.SendingUserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Sending user cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotEqual(requestingUser)
            .WithMessage("User cannot send message to themselves");
        
        // Ensure sender of message is the user requesting the message to be sent
        RuleFor(x => x.SendingUserId)
            .Equal(requestingUser)
            .WithMessage("The sending user must be the requesting user");

        // Prevent sending messages to people who arent friends
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                var isFriends = friends.Any(userFriend => userFriend.FriendId == request.ReceivingUserId);
                if (!isFriends)
                {
                    context.AddFailure("Cannot send message to a user you are not friends with");
                }
            });
    }
}