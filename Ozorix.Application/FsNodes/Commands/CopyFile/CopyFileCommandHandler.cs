using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.CopyFile;

public class CopyFileCommandHandler(IFsService S3FsService)
    : IRequestHandler<CopyFileCommand, ErrorOr<CopyFileCommandResponse>>
{
    public async Task<ErrorOr<CopyFileCommandResponse>> Handle(CopyFileCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.CopyFile(request.Path, request.NewPath);

        return new CopyFileCommandResponse(true);
    }
}
