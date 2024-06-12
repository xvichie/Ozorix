using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Queries.GetCurrentDirectory;

public class GetWorkingDirectoryQueryHandler(IUserCacheService UserCacheService)
    : IRequestHandler<GetWorkingDirectoryQuery, ErrorOr<string>>
{
    public Task<ErrorOr<string>> Handle(GetWorkingDirectoryQuery request, CancellationToken cancellationToken)
    {
        var currentDirectory = UserCacheService.GetCurrentDirectory(request.UserId);
        return Task.FromResult<ErrorOr<string>>(currentDirectory);
    }
}
