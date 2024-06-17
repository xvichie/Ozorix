using FluentValidation;

namespace Ozorix.Application.Authentication.Commands.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
    }
}
