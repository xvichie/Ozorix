using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.DeleteFile;

public class DeleteFileCommandHandler(IFsService S3FsService)
    : IRequestHandler<DeleteFileCommand, ErrorOr<DeleteFileCommandResponse>>
{
    public async Task<ErrorOr<DeleteFileCommandResponse>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.DeleteFile(request.Path);

        return new DeleteFileCommandResponse(true);
    }
}
