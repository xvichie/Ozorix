using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Queries.GetInfo;

public class GetInfoQueryHandler(IFsService S3FsService)
    : IRequestHandler<GetInfoQuery, ErrorOr<GetInfoQueryResponse>>
{
    public async Task<ErrorOr<GetInfoQueryResponse>> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        var metadata = await S3FsService.GetInfo(request.Path);

        var response = new GetInfoQueryResponse(
            Name: metadata.Name,
            Path: metadata.Path,
            Size: metadata.Size,
            MimeType: metadata.MimeType,
            CreatedDateTime: metadata.CreatedDateTime,
            UpdatedDateTime: metadata.UpdatedDateTime
        );

        return response;
    }
}
