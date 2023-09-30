//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Shurly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateShortenedUrlCommandHandler, CreateShortenedUrlCommandHandler>();
        services.AddScoped<IGetByShortCodeQueryHandler, GetByShortCodeQueryHandler>();

        services.AddScoped<CreateShortendUrlCommandValidator>();

        return services;
    }
}
