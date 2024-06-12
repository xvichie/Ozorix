using Ozorix.Domain.UserAggregate;

namespace Ozorix.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);