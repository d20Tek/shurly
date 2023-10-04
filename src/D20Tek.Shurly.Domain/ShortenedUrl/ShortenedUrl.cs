//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class ShortenedUrl : AggregateRoot<ShortenedUrlId>
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
        : base(id)
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

    public void ChangeLongUrl(LongUrl longUrl)
    {
        LongUrl = longUrl;
        UrlMetadata.Modified();
    }

    public void ChangeSummary(Summary summary)
    {
        Summary = summary;
        UrlMetadata.Modified();
    }

    public void ChangePublishOn(DateTime publishOn) =>
        UrlMetadata.UpdatePublishOn(publishOn);

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
