//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder ConfigureMiddlewarePipeline(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        // todo: add D20Tek.Minimal.Endpoints package
        //app.UseExceptionHandler<EndpointExceptionHandler>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
