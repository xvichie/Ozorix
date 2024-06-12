using System.Text;

using Ozorix.Application.Common.Interfaces.Authentication;
using Ozorix.Application.Common.Interfaces.Persistence;
using Ozorix.Application.Common.Interfaces.Services;
using Ozorix.Infrastructure.Authentication;
using Ozorix.Infrastructure.Persistence;
using Ozorix.Infrastructure.Persistence.Interceptors;
using Ozorix.Infrastructure.Persistence.Repositories;
using Ozorix.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Amazon.S3;

namespace Ozorix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddAuth(configuration)
            .AddPersistance();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddAWS(configuration);

        services.AddMemoryCache();

        return services;
    }

    public static IServiceCollection AddPersistance(
        this IServiceCollection services)
    {
        services.AddDbContext<OzorixDbContext>(options =>
            options.UseSqlServer("Server=localhost;Database=Ozorix;User Id=SA;Password=amiko123!;TrustServerCertificate=true"));

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFsNodeRepository, FsNodeRepository>();

        return services;
    }

    public static IServiceCollection AddAuth(
            this IServiceCollection services,
            ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        return services;
    }

    public static IServiceCollection AddAWS(
     this IServiceCollection services,
     IConfiguration configuration
 )
    {
        // Configure AWS options from configuration
        var awsOptions = configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(awsOptions);

        // Register Amazon S3 client using credentials from configuration
        services.AddScoped<IAmazonS3>(provider =>
        {
            var awsAccessKey = configuration["AWS:AccessKey"];
            var awsSecretKey = configuration["AWS:SecretKey"];
            var awsRegion = configuration["AWS:Region"];

            var s3ClientConfig = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsRegion)
            };

            return new AmazonS3Client(awsAccessKey, awsSecretKey, s3ClientConfig);
        });

        // Register S3FsService
        services.AddScoped<IFsService, S3FsService>(provider =>
        {
            var s3Client = provider.GetRequiredService<IAmazonS3>();
            var bucketName = configuration.GetValue<string>("AWS:BucketName");
            return new S3FsService(s3Client, bucketName);
        });

        return services;
    }

    public static IServiceCollection AddMemoryCache(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddSingleton<IUserCacheService, UserCacheService>();

        return services;
    }
}