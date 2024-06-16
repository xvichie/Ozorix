using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Queries.GetCurrentDirectory;

public class GetWorkingDirectoryQueryHandler(IUserCacheService UserCacheService)
    : IRequestHandler<GetWorkingDirectoryQuery, ErrorOr<string>>
{
    public Task<ErrorOr<string>> Handle(GetWorkingDirectoryQuery request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Task.FromResult<ErrorOr<string>>(Errors.User.UserNotFoundInCache);
        }

        var currentDirectory = UserCacheService.GetCurrentDirectory(request.UserId);
        return Task.FromResult<ErrorOr<string>>(currentDirectory);
    }
}
