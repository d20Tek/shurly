//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using D20Tek.Authentication.Individual.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace D20Tek.Shurly.Web;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureAppSettings(configuration);

        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();

        return services;
    }

    private static IServiceCollection ConfigureAppSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtClientSettings();
        configuration.Bind(nameof(JwtClientSettings), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        var endpointSettings = new ServiceEndpointSettings();
        configuration.Bind(nameof(ServiceEndpointSettings), endpointSettings);
        services.AddSingleton(Options.Create(endpointSettings));

        return services;
    }
}
