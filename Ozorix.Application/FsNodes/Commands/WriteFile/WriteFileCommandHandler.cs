using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Commands.WriteFile;

public class WriteFileCommandHandler(IFsService S3FsService)
    : IRequestHandler<WriteFileCommand, ErrorOr<WriteFileCommandResponse>>
{
    public async Task<ErrorOr<WriteFileCommandResponse>> Handle(WriteFileCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.WriteFile(request.Path, request.File, request.UserId);

        return new WriteFileCommandResponse(true);
    }
}
