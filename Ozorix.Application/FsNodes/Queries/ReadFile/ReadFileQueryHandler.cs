using ErrorOr;
using HeyRed.Mime;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Errors = Ozorix.Domain.Common.DomainErrors.Errors;


namespace Ozorix.Application.FsNodes.Queries.ReadFile;

public class ReadFileQueryHandler(IFsService S3FsService, IUserCacheService UserCacheService)
    : IRequestHandler<ReadFileQuery, ErrorOr<ReadFileQueryResponse>>
{
    public async Task<ErrorOr<ReadFileQueryResponse>> Handle(ReadFileQuery request, CancellationToken cancellationToken)
    {
        if (!UserCacheService.IsUserCached(request.UserId))
        {
            return Errors.User.UserNotFoundInCache;
        }

        var content = await S3FsService.ReadFile(request.Path, request.UserId);

        var fileName = System.IO.Path.GetFileName(request.Path);
        
        var contentType = MimeTypesMap.GetMimeType(fileName);

        var response = new ReadFileQueryResponse(fileName, contentType, content);

        return response;
    }
}
