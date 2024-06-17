using Ozorix.Domain.Common.Models;

namespace Ozorix.Domain.UserAggregate.Events;

public record UserRegisteredEvent(string UserId) : IDomainEvent;
