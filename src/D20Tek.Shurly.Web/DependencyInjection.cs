//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using D20Tek.Authentication.Individual.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace D20Tek.Shurly.Web;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IWebAssemblyHostEnvironment hostEnvironment)
    {
        services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });

        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();

        return services;
    }
}
