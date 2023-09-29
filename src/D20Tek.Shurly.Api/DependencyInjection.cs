//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Exceptions;
using D20Tek.Shurly.Infrastructure;

namespace D20Tek.Shurly.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry()
                .AddAuthenticationApiEndpoints()
                .AddInfrastructureServices(configuration);

        // add ApiEndpoint definitions to the container
        services.AddApiEndpointsFromAssembly(
            typeof(DependencyInjection).Assembly,
            ServiceLifetime.Scoped);

        // add swagger services
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // add CORS settings
        services.AddCors(options =>
            options.AddDefaultPolicy(config =>
                config.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()));

        return services;
    }

    public static IApplicationBuilder ConfigureMiddlewarePipeline(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler<EndpointExceptionHandler>();
        app.UseApiEndpointLogging();

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
