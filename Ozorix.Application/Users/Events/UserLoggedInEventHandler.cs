using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.UserAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.Users.Events;

public class UserLoggedInEventHandler(IUserCacheService UserCacheService)
    : INotificationHandler<UserLoggedInEvent>
{
    public Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        UserCacheService.AddUser(notification.UserId);
        UserCacheService.SetCurrentDirectory(notification.UserId, notification.UserId);
        return Task.CompletedTask;
    }
}