using ErrorOr;
using HeyRed.Mime;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Queries.ReadFile;

public class ReadFileQueryHandler(IFsService S3FsService)
    : IRequestHandler<ReadFileQuery, ErrorOr<ReadFileQueryResponse>>
{
    public async Task<ErrorOr<ReadFileQueryResponse>> Handle(ReadFileQuery request, CancellationToken cancellationToken)
    {
        var content = await S3FsService.ReadFile(request.Path);

        var fileName = System.IO.Path.GetFileName(request.Path);
        
        var contentType = MimeTypesMap.GetMimeType(fileName);

        var response = new ReadFileQueryResponse(fileName, contentType, content);

        return response;
    }
}
