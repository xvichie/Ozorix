using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Commands.DeleteFile;

public class DeleteFileCommandHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<DeleteFileCommand, ErrorOr<DeleteFileCommandResponse>>
{
    public async Task<ErrorOr<DeleteFileCommandResponse>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        if (!await S3FsService.KeyExists(request.Path, request.UserId))
        {
            return Errors.Fs.PathNotFound;
        }
        await S3FsService.DeleteFile(request.Path, request.UserId);

        return new DeleteFileCommandResponse(true);
    }
}
