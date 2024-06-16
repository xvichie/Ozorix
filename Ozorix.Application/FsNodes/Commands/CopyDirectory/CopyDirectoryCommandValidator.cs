using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.CopyDirectory;

public class CopyDirectoryCommandValidator : AbstractValidator<CopyDirectoryCommand>
{
    public CopyDirectoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
        RuleFor(x => x.NewPath).NotEmpty();
    }
}
