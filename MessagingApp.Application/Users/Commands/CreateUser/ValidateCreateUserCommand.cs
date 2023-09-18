using FluentValidation;

namespace MessagingApp.Application.Users.Commands.CreateUser;

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