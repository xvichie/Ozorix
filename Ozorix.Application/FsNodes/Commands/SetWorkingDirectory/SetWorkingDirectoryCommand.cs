using ErrorOr;
using MediatR;

namespace Ozorix.Application.FsNodes.Commands.SetWorkingDirectory;

public record SetWorkingDirectoryCommand(string UserId, string CurrentDirectory) 
    : IRequest<ErrorOr<Unit>>;
