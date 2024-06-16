using FluentValidation;

namespace Ozorix.Application.FsNodes.Commands.MoveFile;

public class MoveFileCommandValidator : AbstractValidator<MoveFileCommand>
{
    public MoveFileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.NewPath).NotEmpty();
    }
}
