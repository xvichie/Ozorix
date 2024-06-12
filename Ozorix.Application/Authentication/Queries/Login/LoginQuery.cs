using Ozorix.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace Ozorix.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;