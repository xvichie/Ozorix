namespace Ozorix.Contracts.FsNodes.ListDirectory;

public record ListDirectoryResponse(IEnumerable<FsNodeDto> FsNodes);
