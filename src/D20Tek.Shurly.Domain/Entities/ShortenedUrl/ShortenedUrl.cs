//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Domain.Entities.ShortenedUrl;

public sealed class ShortenedUrl
{
    public ShortenedUrlId Id { get; }

    public LongUrl LongUrl { get; }

    public Summary Summary { get; }

    public ShortUrlCode ShortUrlCode { get; }

    public UrlMetadata UrlMetadata { get; }

    private ShortenedUrl(
        ShortenedUrlId id,
        LongUrl longUrl,
        Summary summary,
        ShortUrlCode shortUrlCode,
        UrlMetadata urlMetadata)
    {
        Id = id;
        LongUrl = longUrl;
        Summary = summary;
        ShortUrlCode = shortUrlCode;
        UrlMetadata = urlMetadata;
    }

    public static ShortenedUrl Create(
        LongUrl longUrl,
        Summary summary,
        ShortUrlCode shortUrlCode,
        AccountId creatorId)
    {
        return new ShortenedUrl(
            ShortenedUrlId.Create(),
            longUrl,
            summary,
            shortUrlCode,
            UrlMetadata.Create(creatorId));
    }

    public static ShortenedUrl Hydrate(
        ShortenedUrlId id,
        LongUrl longUrl,
        Summary summary,
        ShortUrlCode shortUrlCode,
        UrlMetadata urlMetadata)
    {
        return new ShortenedUrl(id, longUrl, summary, shortUrlCode, urlMetadata);
    }
}
