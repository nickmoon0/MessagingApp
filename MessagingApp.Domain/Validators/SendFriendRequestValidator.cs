using System.Data;
using FluentValidation;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

public class SendFriendRequestValidator : AbstractValidator<FriendRequest>
{
    public SendFriendRequestValidator(Guid requestingUserId, 
        IReadOnlyCollection<FriendRequest> sentRequests, 
        IReadOnlyCollection<FriendRequest> receivedRequests,
        IReadOnlyCollection<UserFriend> friends)
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
                var sentReq = sentRequests.Any(x => 
                    x.FromUserId == request.FromUserId && 
                    x.ToUserId == request.ToUserId && 
                    x.Status == FriendRequestStatus.Pending);
                var receivedReq = receivedRequests.Any(x => 
                    x.FromUserId == request.ToUserId && 
                    x.ToUserId == request.FromUserId && 
                    x.Status == FriendRequestStatus.Pending);
                
                if (sentReq)
                {
                    context.AddFailure("This friend request has already been sent.");
                }
                if (receivedReq)
                {
                    context.AddFailure("A friend request from this user has already been received.");
                }
            });

        RuleFor(x => x)
            .Custom((request, context) =>
            {
                var friendExists = friends.Any(x => x.FriendId == request.ToUserId);
                if (friendExists)
                {
                    context.AddFailure("Already friends with this user");
                }
            });
    }
}