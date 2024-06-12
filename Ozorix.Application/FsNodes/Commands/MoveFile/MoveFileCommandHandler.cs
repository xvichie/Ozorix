using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Commands.MoveFile;

public class MoveFileCommandHandler(IFsService S3FsService)
    : IRequestHandler<MoveFileCommand, ErrorOr<MoveFileCommandResponse>>
{
    public async Task<ErrorOr<MoveFileCommandResponse>> Handle(MoveFileCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.CopyFile(request.Path, request.NewPath);
        await S3FsService.DeleteFile(request.Path);

        return new MoveFileCommandResponse(true);
    }
}
