using FluentValidation;
using MessagingApp.Application.Commands;

namespace MessagingApp.Application.Common.Validators;

public class ValidateCreateUserCommand : AbstractValidator<CreateUserCommand>
{
    public ValidateCreateUserCommand()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty()
            .WithMessage("Username cannot be empty when creating a user");
        RuleFor(x => x.Password).NotNull().NotEmpty()
            .WithMessage("Password cannot be empty when creating a user");
    }
}