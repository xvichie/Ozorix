using ErrorOr;
using MediatR;

namespace Ozorix.Application.FsNodes.Commands.CopyDirectory;

public record CopyDirectoryCommand(string Path, string NewPath, string UserId) : IRequest<ErrorOr<CopyDirectoryCommandResponse>>;
