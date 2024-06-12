using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.MoveDirectory;

public class MoveDirectoryCommandHandler(IFsService S3FsService)
    : IRequestHandler<MoveDirectoryCommand, ErrorOr<MoveDirectoryCommandResponse>>
{
    public async Task<ErrorOr<MoveDirectoryCommandResponse>> Handle(MoveDirectoryCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.MoveDirectory(request.Path, request.NewPath);

        return new MoveDirectoryCommandResponse();
    }
}
