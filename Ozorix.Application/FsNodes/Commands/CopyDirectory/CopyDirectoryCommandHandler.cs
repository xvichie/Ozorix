using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Commands.CopyDirectory;

public class CopyDirectoryCommandHandler(IFsService S3FsService) : IRequestHandler<CopyDirectoryCommand, ErrorOr<CopyDirectoryCommandResponse>>
{
    public async Task<ErrorOr<CopyDirectoryCommandResponse>> Handle(CopyDirectoryCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.CopyDirectory(request.Path, request.NewPath);

        return new CopyDirectoryCommandResponse();
    }
}
