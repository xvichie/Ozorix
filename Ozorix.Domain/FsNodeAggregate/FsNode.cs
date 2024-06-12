using Ozorix.Domain.FsNodeAggregate.ValueObjects;
using Ozorix.Domain.Common.Models;
using Ozorix.Domain.UserAggregate.ValueObjects;

namespace Ozorix.Domain.FsNodeAggregate;

public sealed class FsNode : AggregateRoot<FsNodeId, Guid>
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public int Size { get; private set; }
    public string MimeType { get; private set; }
    public UserId UserId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private FsNode(string name, string path, int size, string mimeType, 
        UserId userId, FsNodeId? fsNodeId = null) 
        : base(fsNodeId ?? FsNodeId.CreateUnique())
    {
        Name = name;
        Path = path;
        Size = size;
        MimeType = mimeType;
        UserId = userId;
    }

    public static FsNode Create(string name, string path, int size, string mimeType, UserId userId)
    {
        return new FsNode(name, path, size, mimeType, userId);
    }

    private FsNode()
    {

    }
}
