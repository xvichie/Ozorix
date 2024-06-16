using FluentValidation;

namespace Ozorix.Application.FsNodes.Commands.SetWorkingDirectory;

public class SetWorkingDirectoryCommandValidator : AbstractValidator<SetWorkingDirectoryCommand>
{
    public SetWorkingDirectoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CurrentDirectory).NotEmpty();
    }
}
