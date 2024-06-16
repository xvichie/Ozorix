using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Application.FsNodes.Commands.CreateDirectory;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.MoveDirectory;

public class MoveDirectoryCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<MoveDirectoryCommand, ErrorOr<MoveDirectoryCommandResponse>>
{
    public async Task<ErrorOr<MoveDirectoryCommandResponse>> Handle(MoveDirectoryCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.Path, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }

        await S3FsService.MoveDirectory(request.Path, request.NewPath, request.UserId);


        return new MoveDirectoryCommandResponse(
            UserCacheService.GetCurrentDirectory(request.UserId) + '/' + request.NewPath
        );
    }
}
