//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.Entities.ShortenedUrl;

public sealed class ShortenedUrl : Entity<ShortenedUrlId>
{
    public LongUrl LongUrl { get; private set; } = default!;

    public Summary Summary { get; private set; } = default!;

    public ShortUrlCode ShortUrlCode { get; private set; } = default!;

    public UrlMetadata UrlMetadata { get; private set; } = default!;

    private ShortenedUrl(
        ShortenedUrlId id,
        LongUrl longUrl,
        Summary summary,
        ShortUrlCode shortUrlCode,
        UrlMetadata urlMetadata)
        : base( id )
    {
        LongUrl = longUrl;
        Summary = summary;
        ShortUrlCode = shortUrlCode;
        UrlMetadata = urlMetadata;
    }

    private ShortenedUrl()
        : base(ShortenedUrlId.Create())
    {
    }

    public void Update(LongUrl longUrl, Summary summary, DateTime? publishOn)
    {
        LongUrl = longUrl;
        Summary = summary;

        if (publishOn is not null)
        {
            
        }
    }

    public static ShortenedUrl Create(
        LongUrl longUrl,
        Summary summary,
        ShortUrlCode shortUrlCode,
        AccountId creatorId,
        DateTime? publishOn)
    {
        return new ShortenedUrl(
            ShortenedUrlId.Create(),
            longUrl,
            summary,
            shortUrlCode,
            UrlMetadata.Create(creatorId, publishOn));
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
