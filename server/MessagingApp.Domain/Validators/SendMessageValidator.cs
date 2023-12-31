﻿using FluentValidation;
using FluentValidation.Results;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

/// <summary>
/// Checks for:
///     - Message cant be null or empty
///     - Receiving and sending user cannot be null
///     - Users cant send messages to themselves
///     - User must be authorised to send message from sending user
///     - Must be friends with receiving user to send them a message
/// </summary>
public class SendMessageValidator : AbstractValidator<Message>
{
    public SendMessageValidator(
        Guid requestingUser,
        IReadOnlyCollection<UserFriend> friends)
    {
        RuleFor(x => x.Text)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("Message cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("Receiving user cannot be null or empty");
        
        RuleFor(x => x.SendingUserId)
            .NotNull()
            .NotEmpty()
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("Sending user cannot be null or empty");

        RuleFor(x => x.ReceivingUserId)
            .NotEqual(requestingUser)
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("User cannot send message to themselves");
        
        // Ensure sender of message is the user requesting the message to be sent
        RuleFor(x => x.SendingUserId)
            .Equal(requestingUser)
            .WithErrorCode(ErrorCodes.Unauthorised)
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
                    ErrorCode = ErrorCodes.BadRequest
                };
                context.AddFailure(valFailure);
            });
    }
}