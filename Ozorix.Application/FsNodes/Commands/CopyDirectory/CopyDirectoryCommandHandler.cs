using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;


namespace Ozorix.Application.FsNodes.Commands.CopyDirectory;

public class CopyDirectoryCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService) : IRequestHandler<CopyDirectoryCommand, ErrorOr<CopyDirectoryCommandResponse>>
{
    public async Task<ErrorOr<CopyDirectoryCommandResponse>> Handle(CopyDirectoryCommand request, CancellationToken cancellationToken)
    {
        if(!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.Path, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }
        await S3FsService.CopyDirectory(request.Path, request.NewPath, request.UserId);

        return new CopyDirectoryCommandResponse(
            UserCacheService.GetCurrentDirectory(request.UserId) + '/' + request.NewPath
        );
    }
}
