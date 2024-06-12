using Mapster;
using Ozorix.Application.Authentication.Commands.Register;
using Ozorix.Application.Authentication.Common;
using Ozorix.Application.Authentication.Queries.Login;
using Ozorix.Contracts.Authentication;

namespace Ozorix.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.Value.ToString())
            .Map(dest => dest, src => src.User);
    }
}