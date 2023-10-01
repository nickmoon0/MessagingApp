using FluentValidation;
using FluentValidation.Results;
using MessagingApp.Domain.Common;
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
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("Message cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("Receiving user cannot be null or empty");
        
        RuleFor(x => x.SendingUserId)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("Sending user cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotEqual(requestingUser)
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("User cannot send message to themselves");
        
        // Ensure sender of message is the user requesting the message to be sent
        RuleFor(x => x.SendingUserId)
            .Equal(requestingUser)
            .WithErrorCode(DomainErrorCodes.Unauthorised)
            .WithMessage("User is not authorised to send message");

        // Prevent sending messages to people who arent friends
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                var isFriends = friends.Any(userFriend => userFriend.FriendId == request.ReceivingUserId);
                if (isFriends) return;
                var valFailure = new ValidationFailure(nameof(request.ReceivingUserId),
                    "Cannot send message to a user you are not friends with")
                {
                    ErrorCode = DomainErrorCodes.BadRequest
                };
                context.AddFailure(valFailure);
            });
    }
}