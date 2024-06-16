using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Application.FsNodes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;

namespace Ozorix.Application.FsNodes.Queries.ListDirectory;

public class ListDirectoryQueryHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<ListDirectoryQuery, ErrorOr<ListDirectoryQueryResponse>>
{
    public async Task<ErrorOr<ListDirectoryQueryResponse>> Handle(ListDirectoryQuery request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        var fsNodes = await S3FsService.ListDirectory(request.Path, request.UserId);

        return new ListDirectoryQueryResponse
        {
            FsNodes = fsNodes.Select(f => new FsNodeDto(
                f.Id.Value.ToString(),
                f.Name,
                f.Path,
                f.Size,
                f.MimeType,
                f.UserId.Value.ToString()
            )).ToList()
        };
    }
}
