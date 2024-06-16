using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.DeleteDirectory;

public class DeleteDirectoryCommandValidator : AbstractValidator<DeleteDirectoryCommand>
{
    public DeleteDirectoryCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
    }
}
