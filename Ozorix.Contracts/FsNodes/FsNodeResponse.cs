namespace Ozorix.Contracts.FsNodes;

public class FsNodeResponse(
    string Id,
    string Name,
    string Path,
    string MimeType,
    string UserId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);