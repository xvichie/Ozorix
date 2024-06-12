using Ozorix.Domain.Common.Models;

namespace Ozorix.Domain.UserAggregate.Events;

public record UserLoggedInEvent(string UserId) : IDomainEvent;
