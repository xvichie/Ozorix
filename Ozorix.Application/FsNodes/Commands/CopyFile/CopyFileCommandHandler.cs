using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.CopyFile;

public class CopyFileCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<CopyFileCommand, ErrorOr<CopyFileCommandResponse>>
{
    public async Task<ErrorOr<CopyFileCommandResponse>> Handle(CopyFileCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.Path, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }
        await S3FsService.CopyFile(request.Path, request.NewPath, request.UserId);


        return new CopyFileCommandResponse(
            UserCacheService.GetCurrentDirectory(request.UserId) + '/' + request.NewPath
            );
    }
}
