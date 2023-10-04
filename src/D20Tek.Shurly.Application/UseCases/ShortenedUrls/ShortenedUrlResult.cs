//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls;

public sealed record ShortenedUrlResult(
    Guid Id,
    string LongUrl,
    string ShortUrlCode,
    string Summary,
    int State,
    DateTime PublishOn)
{
    internal static ShortenedUrlResult FromEntity(ShortenedUrl entity)
    {
        return new ShortenedUrlResult(
            entity.Id.Value,
            entity.LongUrl.Value,
            entity.ShortUrlCode.Value,
            entity.Summary.Value,
            (int)entity.UrlMetadata.State,
            entity.UrlMetadata.PublishOn);
    }
}
