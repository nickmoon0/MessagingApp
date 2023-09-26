using System.Data;
using FluentValidation;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

public class FriendRequestValidator : AbstractValidator<FriendRequest>
{
    public FriendRequestValidator(Guid requestingUserId, 
        List<FriendRequest> sentRequests, 
        List<FriendRequest> receivedRequests)
    {
        // Check properties have valid values
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
        
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                if (sentRequests.Any(x => x.FromUserId == request.FromUserId && x.ToUserId == request.ToUserId))
                {
                    context.AddFailure("This friend request has already been sent.");
                }
                if (receivedRequests.Any(x => x.FromUserId == request.ToUserId && x.ToUserId == request.FromUserId))
                {
                    context.AddFailure("A friend request from this user has already been received.");
                }
            });
    }
}