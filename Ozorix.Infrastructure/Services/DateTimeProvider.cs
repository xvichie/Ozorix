using Ozorix.Application.Common.Interfaces.Services;

namespace Ozorix.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}