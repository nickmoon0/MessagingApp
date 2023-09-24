using System.Data;
using FluentValidation;
using MessagingApp.Domain.Aggregates;

namespace MessagingApp.Domain.Validators;

public class FriendRequestValidator : AbstractValidator<FriendRequest>
{
    public FriendRequestValidator(Guid requestingUserId)
    {
        RuleFor(x => x.FromUserId)
            .NotNull()
            .WithMessage("FromUser cannot be null");
        RuleFor(x => x.ToUserId)
            .NotNull()
            .WithMessage("ToUser cannot be null");
        
        RuleFor(x => x.FromUserId)
            .NotEqual(x => x.ToUserId)
            .WithMessage("User cannot send a friend request to themselves");

        RuleFor(x => x.FromUserId)
            .Equal(requestingUserId)
            .WithMessage("User is not authorised to send friend request");
    }
}