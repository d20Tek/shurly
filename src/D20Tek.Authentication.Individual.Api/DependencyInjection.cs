//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }
}
