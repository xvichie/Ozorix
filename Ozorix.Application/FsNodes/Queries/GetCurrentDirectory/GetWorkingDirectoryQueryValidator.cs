using FluentValidation;

namespace Ozorix.Application.FsNodes.Queries.GetCurrentDirectory;

public class GetWorkingDirectoryQueryValidator : AbstractValidator<GetWorkingDirectoryQuery>
{
    public GetWorkingDirectoryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
