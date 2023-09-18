using FluentValidation;

namespace MessagingApp.Application.Users.Queries.AuthenticateUser;

public class ValidateAuthenticateUserQuery : AbstractValidator<AuthenticateUserQuery>
{
    public ValidateAuthenticateUserQuery()
    {
        RuleFor(x => x.Username).NotEmpty().NotNull()
            .WithMessage("Username cannot be null or empty when authenticating");
        RuleFor(x => x.Password).NotEmpty().NotNull()
            .WithMessage("Password cannot be null or empty when authenticating");
    }    
}