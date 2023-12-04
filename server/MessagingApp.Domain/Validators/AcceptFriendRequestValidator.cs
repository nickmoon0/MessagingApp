using FluentValidation;
using FluentValidation.Results;
using MessagingApp.Domain.Common;
using MessagingApp.Domain.Entities;

namespace MessagingApp.Domain.Validators;

/// <summary>
/// Checks for:
///     - Friend request has to be sent to accepting user
///     - Friend request ID cannot be null or empty
///     - Friend request cant be accepted or declined already
///     - Friend request has to exist
/// </summary>
public class AcceptFriendRequestValidator : AbstractValidator<FriendRequest>
{
    public AcceptFriendRequestValidator(Guid requestingUser,
        IReadOnlyCollection<FriendRequest> receivedRequests)
    {
        RuleFor(x => x.ToUserId)
            .Equal(requestingUser)
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("Cannot accept a request that was not sent to user");

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .NotNull()
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("FriendRequest ID Guid cannot be null or empty");

        RuleFor(x => x.Status)
            .Equal(FriendRequestStatus.Pending)
            .WithErrorCode(ErrorCodes.BadRequest)
            .WithMessage("Cannot accept a friend request that is already accepted or declined");
        
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                var requestExists = receivedRequests.Any(x => x.Id == request.Id);
                if (requestExists) return;
                
                var valFailure = new ValidationFailure(nameof(request.Id), "Friend request does not exist")
                {
                    ErrorCode = ErrorCodes.NotFound
                };
                context.AddFailure(valFailure);
            });
    }
}