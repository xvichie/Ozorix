using Ozorix.Domain.UserAggregate;

namespace Ozorix.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}