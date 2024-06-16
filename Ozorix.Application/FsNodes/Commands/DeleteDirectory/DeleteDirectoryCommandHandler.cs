using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.DeleteDirectory;

public class DeleteDirectoryCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService) 
    : IRequestHandler<DeleteDirectoryCommand, ErrorOr<DeleteDirectoryCommandResponse>>
{
    public async Task<ErrorOr<DeleteDirectoryCommandResponse>> Handle(DeleteDirectoryCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.Path, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }
        await S3FsService.DeleteDirectory(request.Path, request.UserId);

        return new DeleteDirectoryCommandResponse(true);
    }
}
