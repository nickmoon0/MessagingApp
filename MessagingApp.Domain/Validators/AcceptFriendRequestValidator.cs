using FluentValidation;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

public class AcceptFriendRequestValidator : AbstractValidator<FriendRequest>
{
    public AcceptFriendRequestValidator(Guid requestingUser,
        IReadOnlyCollection<FriendRequest> receivedRequests)
    {
        RuleFor(x => x.ToUserId)
            .Equal(requestingUser)
            .WithMessage("Cannot accept a request that was not sent to user");

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .NotNull()
            .WithMessage("FriendRequest ID Guid cannot be null or empty");

        RuleFor(x => x.Status)
            .Equal(FriendRequestStatus.Pending)
            .WithMessage("Cannot accept a friend request that is already accepted or declined");
        
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                var requestExists = receivedRequests.Any(x => x.Id == request.Id);
                if (!requestExists)
                {
                    context.AddFailure("Friend request does not exist");
                }
            });
    }
}