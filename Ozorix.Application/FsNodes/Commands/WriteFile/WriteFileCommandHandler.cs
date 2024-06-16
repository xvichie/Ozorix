using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.WriteFile;

public class WriteFileCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<WriteFileCommand, ErrorOr<WriteFileCommandResponse>>
{
    public async Task<ErrorOr<WriteFileCommandResponse>> Handle(WriteFileCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        await S3FsService.WriteFile(request.Path, request.File, request.UserId);

        return new WriteFileCommandResponse(true);
    }
}
