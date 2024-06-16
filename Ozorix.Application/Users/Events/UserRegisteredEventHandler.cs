using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.UserAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.Users.Events;

public class UserRegisteredEventHandler(IFsService S3FsService)
    : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        await S3FsService.CreateDirectory(notification.UserId, notification.UserId);
    }
}
