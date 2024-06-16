using FluentValidation;

namespace Ozorix.Application.FsNodes.Queries.GetInfo;

public class GetInfoQueryValidator : AbstractValidator<GetInfoQuery>
{
    public GetInfoQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Path).NotEmpty();
    }
}
