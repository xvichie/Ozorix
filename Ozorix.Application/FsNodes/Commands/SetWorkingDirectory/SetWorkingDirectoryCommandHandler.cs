using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozorix.Application.FsNodes.Commands.SetWorkingDirectory;

public class SetWorkingDirectoryCommandHandler(IUserCacheService UserCacheService)
    : IRequestHandler<SetWorkingDirectoryCommand, ErrorOr<Unit>>
{
    public Task<ErrorOr<Unit>> Handle(SetWorkingDirectoryCommand request, CancellationToken cancellationToken)
    {
        UserCacheService.SetCurrentDirectory(request.UserId, request.CurrentDirectory);
        return Task.FromResult<ErrorOr<Unit>>(Unit.Value);
    }
}
