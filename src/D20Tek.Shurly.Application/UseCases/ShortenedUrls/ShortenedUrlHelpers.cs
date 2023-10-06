//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls;

internal static class ShortenedUrlHelpers
{
    internal static async Task<Result<ShortenedUrl>> GetByIdForOwner(
        IShortenedUrlRepository repository,
        Guid shortUrlId,
        Guid ownerId)
    {
        var id = ShortenedUrlId.Create(shortUrlId);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
        {
            return DomainErrors.EntityNotFound(nameof(ShortenedUrl), shortUrlId);
        }

        if (entity.UrlMetadata.OwnerId.Value != ownerId)
        {
            return DomainErrors.ShortUrlNotOwner;
        }

        return entity;
    }
}
