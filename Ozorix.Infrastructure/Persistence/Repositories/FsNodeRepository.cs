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

    public async Task<bool> ExistsAsync(FsNodeId fsNodeId)
    {
        //return await _dbContext.FsNodes.AnyAsync(fsNode => fsNode.Id == fsNodeId);
        throw new NotImplementedException();
    }

    public async Task<FsNode?> GetByIdAsync(FsNodeId fsNodeId)
    {
        //return await _dbContext.FsNodes.FirstOrDefaultAsync(fsNode => fsNode.Id == fsNodeId);
        throw new NotImplementedException();

    }

    public async Task UpdateAsync(FsNode fsNode)
    {
        //_dbContext.FsNodes.Update(fsNode);
        //await _dbContext.SaveChangesAsync();
        throw new NotImplementedException();

    }
}