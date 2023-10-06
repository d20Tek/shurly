//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.PublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.UnpublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Shurly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateShortenedUrlCommandHandler, CreateShortenedUrlCommandHandler>();
        services.AddScoped<IGetByOwnerQueryHandler, GetByOwnerQueryHandler>();
        services.AddScoped<IGetByIdQueryHandler, GetByIdQueryHandler>();
        services.AddScoped<IGetByShortCodeQueryHandler, GetByShortCodeQueryHandler>();
        services.AddScoped<IUpdateShortenedUrlCommandHandler, UpdateShortenedUrlCommandHandler>();
        services.AddScoped<IPublishShortenedUrlCommandHandler, PublishShortenedUrlCommandHandler>();
        services.AddScoped<IUnpublishShortenedUrlCommandHandler, UnpublishShortenedUrlCommandHandler>();
        services.AddScoped<IDeleteShortenedUrlCommandHandler, DeleteShortenedUrlCommandHandler>();

        services.AddScoped<CreateShortenedUrlCommandValidator>();
        services.AddScoped<UpdateShortenedUrlCommandValidator>();

        return services;
    }
}
