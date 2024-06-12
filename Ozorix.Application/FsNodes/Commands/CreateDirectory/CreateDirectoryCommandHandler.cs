using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Commands.CreateDirectory;

public class CreateDirectoryCommandHandler(IFsService S3FsService) : IRequestHandler<CreateDirectoryCommand, ErrorOr<CreateDirectoryCommandResponse>>
{
    public async Task<ErrorOr<CreateDirectoryCommandResponse>> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.CreateDirectory(request.Path);
        //await _fsNodeRepository.AddAsync(fsNode);

        return new CreateDirectoryCommandResponse();
    }
}
