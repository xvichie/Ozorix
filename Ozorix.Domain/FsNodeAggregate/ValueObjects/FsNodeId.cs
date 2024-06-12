using Ozorix.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Domain.FsNodeAggregate.ValueObjects;

public sealed class FsNodeId : AggregateRootId<Guid>
{
    private FsNodeId(Guid value) : base(value) 
    {

    }

    public static FsNodeId CreateUnique()
    {
        return new FsNodeId(Guid.NewGuid());
    }

    public static FsNodeId Create(Guid value)
    {
        return new FsNodeId(value);
    }
}
