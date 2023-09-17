using FluentValidation;
using MessagingApp.Application.Queries;

namespace MessagingApp.Application.Common.Validators;

public class ValidateRetrieveUserQuery : AbstractValidator<RetrieveUserQuery>
{
    public ValidateRetrieveUserQuery()
    {
        RuleFor(x => x).Must(HaveOneNonNullProperty)
            .WithMessage($"No username or Id was specified for retrieving user");
    }

    private static bool HaveOneNonNullProperty(RetrieveUserQuery query)
    {
        return (query.Id is not null && query.Id != Guid.Empty)  || !string.IsNullOrEmpty(query.Username);
    }
}