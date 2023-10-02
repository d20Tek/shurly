//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Shurly.Infrastructure;

public static class DependencyInjection
{
    private const string _defaultDbConnectionKey = "ShurlyConnection";

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);

        services.AddSingleton<IUrlShorteningService, UrlShorteningService>();
        services.AddSingleton<IShortenedUrlRepository, ShortenedUrlRepository>();

        return services;
    }

    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(_defaultDbConnectionKey) ??
            throw new InvalidOperationException(
                $"Connection string '{_defaultDbConnectionKey}' not found.");

        services.AddDbContext<ShurlyDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("D20Tek.Shurly.Infrastructure")),
            ServiceLifetime.Singleton);

        return services;
    }
}
