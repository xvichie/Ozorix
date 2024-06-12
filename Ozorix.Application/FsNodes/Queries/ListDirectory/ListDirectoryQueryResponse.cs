using Ozorix.Application.FsNodes.Dto;
using Ozorix.Domain.FsNodeAggregate;

namespace Ozorix.Application.FsNodes.Queries.ListDirectory;

public record ListDirectoryQueryResponse
{
    public List<FsNodeDto> FsNodes { get; init; } = new List<FsNodeDto>();
}
