//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Services;
using Microsoft.Extensions.Options;

namespace D20Tek.Shurly.Web;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureAppSettings(configuration);

        services.AddScoped<ShurlyApiService>();

        return services;
    }

    private static IServiceCollection ConfigureAppSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var endpointSettings = new ServiceEndpointSettings();
        configuration.Bind(nameof(ServiceEndpointSettings), endpointSettings);
        services.AddSingleton(Options.Create(endpointSettings));

        return services;
    }
}
