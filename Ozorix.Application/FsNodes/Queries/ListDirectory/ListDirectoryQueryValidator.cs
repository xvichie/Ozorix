using FluentValidation;

namespace Ozorix.Application.FsNodes.Queries.ListDirectory;

public class ListDirectoryQueryValidator : AbstractValidator<ListDirectoryQuery>
{
    public ListDirectoryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
    }
}
