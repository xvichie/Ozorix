using FluentValidation;

namespace Ozorix.Application.FsNodes.Commands.CopyFile;

public class CopyFileCommandValidator : AbstractValidator<CopyFileCommand>
{
    public CopyFileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.NewPath).NotEmpty();
    }
}
