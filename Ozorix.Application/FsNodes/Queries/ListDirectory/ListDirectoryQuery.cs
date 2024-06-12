using ErrorOr;
using MediatR;

namespace Ozorix.Application.FsNodes.Queries.ListDirectory;

public record ListDirectoryQuery(string Path, string UserId)
    : IRequest<ErrorOr<ListDirectoryQueryResponse>>;
