using Ozorix.Domain.FsNodeAggregate;
using System.Collections;

namespace Ozorix.Contracts.FsNodes.ListDirectory;

public record ListDirectoryResponse(IEnumerable<FsNodeDto> FsNodes);
