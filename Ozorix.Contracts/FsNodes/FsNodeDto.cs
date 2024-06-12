namespace Ozorix.Contracts.FsNodes;

public record FsNodeDto(
    string Id,
    string Name,
    string Path, 
    int Size, 
    string MimeType,
    string UserId);
