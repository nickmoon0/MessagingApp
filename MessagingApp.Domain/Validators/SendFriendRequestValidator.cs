using FluentValidation;
using FluentValidation.Results;
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
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("FromUser cannot be null");
        
        RuleFor(x => x.ToUserId)
            .NotNull()
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("ToUser cannot be null");
        
        RuleFor(x => x.FromUserId)
            .NotEqual(x => x.ToUserId)
            .WithErrorCode(DomainErrorCodes.BadRequest)
            .WithMessage("User cannot send a friend request to themselves");

        RuleFor(x => x.FromUserId)
            .Equal(requestingUserId)
            .WithErrorCode(DomainErrorCodes.Unauthorised)
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
                    var valFailure = new ValidationFailure(nameof(request.ToUserId),
                        "This friend request has already been sent.")
                    {
                        ErrorCode = DomainErrorCodes.BadRequest
                    };
                    context.AddFailure(valFailure);
                }
                else if (receivedReq)
                {
                    var valFailure = new ValidationFailure(nameof(request.FromUserId),
                        "A friend request from this user has already been received.")
                    {
                        ErrorCode = DomainErrorCodes.BadRequest
                    };
                    context.AddFailure(valFailure);
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