using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.Common.DomainErrors;

namespace Ozorix.Application.FsNodes.Commands.SetWorkingDirectory;

public class SetWorkingDirectoryCommandHandler(IUserCacheService UserCacheService, IFsService S3FsService)
    : IRequestHandler<SetWorkingDirectoryCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(SetWorkingDirectoryCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.CurrentDirectory, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }

        UserCacheService.SetCurrentDirectory(request.UserId, request.CurrentDirectory);
        return Unit.Value;
    }
}
