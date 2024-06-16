using FluentValidation;

namespace Ozorix.Application.FsNodes.Commands.MoveDirectory;

public class MoveDirectoryCommandValidator : AbstractValidator<MoveDirectoryCommand>
{
    public MoveDirectoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.NewPath).NotEmpty();
    }
}
