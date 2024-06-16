using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;


namespace Ozorix.Application.FsNodes.Commands.MoveFile;

public class MoveFileCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<MoveFileCommand, ErrorOr<MoveFileCommandResponse>>
{
    public async Task<ErrorOr<MoveFileCommandResponse>> Handle(MoveFileCommand request, CancellationToken cancellationToken)
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
        await S3FsService.DeleteFile(request.Path, request.UserId);

        return new MoveFileCommandResponse(
            UserCacheService.GetCurrentDirectory(request.UserId) + '/' + request.NewPath
            );
    }
}
