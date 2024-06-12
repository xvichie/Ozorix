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
        // 1.Validate the user doesn't exist
            if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Create user (generate unique ID) & Persist to DB
        var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

        _userRepository.Add(user);

        // 3. Publish the UserRegisteredDomainEvent
        await _publisher.Publish(new UserRegisteredEvent(user.Id.Value.ToString()), cancellationToken);

        // 4. Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}