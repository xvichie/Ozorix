namespace Ozorix.Application.FsNodes.Queries.GetInfo;

public record GetInfoQueryResponse(
    string Name,
    string Path,
    int Size,
    string MimeType,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
