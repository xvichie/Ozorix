using Microsoft.EntityFrameworkCore;
using Ozorix.Domain.Common.Models;
using Ozorix.Domain.FsNodeAggregate;
using Ozorix.Domain.UserAggregate;
using Ozorix.Infrastructure.Persistence.Interceptors;

namespace Ozorix.Infrastructure.Persistence;

public class OzorixDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public OzorixDbContext(DbContextOptions<OzorixDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<FsNode> FsNodes { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(OzorixDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}