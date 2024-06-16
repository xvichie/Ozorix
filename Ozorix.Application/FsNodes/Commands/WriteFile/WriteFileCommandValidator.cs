using FluentValidation;

namespace Ozorix.Application.FsNodes.Commands.WriteFile;

public class WriteFileCommandValidator : AbstractValidator<WriteFileCommand>
{
    public WriteFileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
    }
}
