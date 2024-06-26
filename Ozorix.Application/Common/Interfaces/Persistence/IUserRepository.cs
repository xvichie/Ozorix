using Ozorix.Domain.UserAggregate;

namespace Ozorix.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}