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
    Task UpdateAsync(FsNode fsNode);
    Task AddAsync(FsNode fsNode);
    Task<FsNode?> GetByIdAsync(FsNodeId fsNodeId);
    Task<bool> ExistsAsync(FsNodeId fsNodeId);
}
