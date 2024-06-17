
using ErrorOr;
using MediatR;

namespace Ozorix.Application.Authentication.Commands.Logout;

public record LogoutCommand(string Email) 
    : IRequest<ErrorOr<LogoutCommandResponse>>;
