using Microsoft.AspNetCore.Mvc.Infrastructure;
using Ozorix.Api.Common.Errors;
using Ozorix.Api.Common.Mapping;

namespace Ozorix.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMemoryCache();
        services.AddSingleton<ProblemDetailsFactory, OzorixProblemDetailsFactory>();
        services.AddMappings();
        return services;
    }
}