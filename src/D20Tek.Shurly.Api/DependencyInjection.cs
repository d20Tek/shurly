﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Exceptions;

namespace D20Tek.Shurly.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services)
    {
        services.AddAuthenticationApiEndpoints();

        // add ApiEndpoint definitions to the container
        services.AddApiEndpointsFromAssembly(
            typeof(DependencyInjection).Assembly,
            ServiceLifetime.Scoped);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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
        app.UseExceptionHandler<EndpointExceptionHandler>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
