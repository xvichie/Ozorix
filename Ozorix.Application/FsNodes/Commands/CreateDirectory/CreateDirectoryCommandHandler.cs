using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Application.FsNodes.Commands.CopyDirectory;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.CreateDirectory;

public class CreateDirectoryCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService) : IRequestHandler<CreateDirectoryCommand, ErrorOr<CreateDirectoryCommandResponse>>
{
    public async Task<ErrorOr<CreateDirectoryCommandResponse>> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }
        await S3FsService.CreateDirectory(request.Path, request.UserId);

        await Console.Out.WriteLineAsync(UserCacheService.GetCurrentDirectory(request.UserId) + request.Path);

        return new CreateDirectoryCommandResponse(
           UserCacheService.GetCurrentDirectory(request.UserId) + '/' + request.Path
        );
    }
}
