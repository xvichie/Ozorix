using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.FsNodeAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.Common.Interfaces.Persistence;

public interface IFsNodeRepository
{
    Task AddAsync(FsNode fsNode);
}
