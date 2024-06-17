using ErrorOr;
using MediatR;
using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Domain.Common.DomainErrors;

namespace Ozorix.Application.Authentication.Commands.Logout;

public class LogoutCommandHandler(IUserCacheService UserCacheService, IUserRepository UserRepository)
    : IRequestHandler<LogoutCommand, ErrorOr<LogoutCommandResponse>>
{
    public async Task<ErrorOr<LogoutCommandResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userInDb = UserRepository.GetUserByEmail(request.Email);

        if (userInDb == null)
        {
            return Errors.User.UserNotFoundInCache;
        }

        if(!UserCacheService.IsUserCached(userInDb.Id.Value.ToString()))
        {
            return Errors.User.UserNotFoundInCache;
        }

        UserCacheService.RemoveUser(userInDb.Id.Value.ToString());

        return new LogoutCommandResponse { Success = true };
    }
}
