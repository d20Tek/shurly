//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Authentication.Individual.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationApiEndpoints(
        this IServiceCollection services)
    {
        // add Authentication ApiEndpoint definitions to the container
        services.AddApiEndpointsFromAssembly(
            typeof(DependencyInjection).Assembly,
            ServiceLifetime.Scoped);

        return services;
    }
}
