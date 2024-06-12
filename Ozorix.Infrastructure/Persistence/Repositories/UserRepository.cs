using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Domain.UserAggregate;

namespace Ozorix.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly OzorixDbContext _dbContext;

    public UserRepository(OzorixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        _dbContext.Add(user);

        _dbContext.SaveChanges();
    }

    public User? GetUserByEmail(string email)
    {
        return _dbContext.Users
            .SingleOrDefault(u => u.Email == email);
    }
}