using Ozorix.Application.Authentication.Common;
using Ozorix.Application.Common.Interfaces.Authentication;
using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Domain.Common.DomainErrors;
using Ozorix.Domain.UserAggregate;

using ErrorOr;

using MediatR;
using Ozorix.Domain.UserAggregate.Events;

namespace Ozorix.Application.Authentication.Commands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPublisher _publisher;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IPublisher publisher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _publisher = publisher;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

        _userRepository.Add(user);

        await _publisher.Publish(new UserRegisteredEvent(user.Id.Value.ToString()), cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}