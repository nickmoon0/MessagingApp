using System.Data;
using FluentValidation;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Domain.Validators;

public class FriendRequestValidator : AbstractValidator<FriendRequest>
{
    public FriendRequestValidator(Guid requestingUserId)
    {
        RuleFor(x => x.FromUser)
            .NotNull()
            .WithMessage("FromUser cannot be null");
        RuleFor(x => x.ToUser)
            .NotNull()
            .WithMessage("ToUser cannot be null");
        
        RuleFor(x => x.FromUser)
            .NotEqual(x => x.ToUser)
            .WithMessage("User cannot send a friend request to themselves");

        RuleFor(x => x.FromUser)
            .Equal(requestingUserId)
            .WithMessage("User is not authorised to send friend request");
    }
}