using Ozorix.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Domain.UserAggregate.Events;

public record UserLoggedInEvent(string UserId) : IDomainEvent;
