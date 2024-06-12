using ErrorOr;
using MediatR;
using Ozorix.Domain.FsNodeAggregate;

namespace Ozorix.Application.FsNodes.Commands.CreateFsNode;

public record CreateFsNodeCommand(
    string Name,
    string Path,
    string MimeType,
    Guid UserId
    ) : IRequest<ErrorOr<FsNode>>;
