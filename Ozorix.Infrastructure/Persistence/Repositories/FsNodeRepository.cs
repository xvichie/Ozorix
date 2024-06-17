using Microsoft.EntityFrameworkCore;
using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.FsNodeAggregate.ValueObjects;

namespace Ozorix.Infrastructure.Persistence.Repositories;

public class FsNodeRepository : IFsNodeRepository
{
    private readonly OzorixDbContext _dbContext;

    public FsNodeRepository(OzorixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FsNode fsNode)
    {
        _dbContext.Add(fsNode);
        await _dbContext.SaveChangesAsync();
    }
}