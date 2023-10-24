//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls;

public sealed record ShortenedUrlResult(
    Guid Id,
    string Title,
    string LongUrl,
    string ShortUrlCode,
    string Summary,
    List<string> Tags,
    int State,
    DateTime CreatedOn,
    DateTime PublishOn)
{
    internal static ShortenedUrlResult FromEntity(ShortenedUrl entity)
    {
        return new ShortenedUrlResult(
            entity.Id.Value,
            entity.Title.Value,
            entity.LongUrl.Value,
            entity.ShortUrlCode.Value,
            entity.Summary.Value,
            entity.UrlMetadata.Tags,
            (int)entity.UrlMetadata.State,
            entity.UrlMetadata.CreatedOn,
            entity.UrlMetadata.PublishOn);
    }
}
