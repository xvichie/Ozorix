using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Application.FsNodes.Commands.DeleteDirectory;

public class DeleteDirectoryCommandHandler(IFsService S3FsService) : IRequestHandler<DeleteDirectoryCommand, ErrorOr<DeleteDirectoryCommandResponse>>
{
    public async Task<ErrorOr<DeleteDirectoryCommandResponse>> Handle(DeleteDirectoryCommand request, CancellationToken cancellationToken)
    {
        await S3FsService.DeleteDirectory(request.Path);
        //await _fsNodeRepository.AddAsync(fsNode);

        return new DeleteDirectoryCommandResponse();
    }
}
