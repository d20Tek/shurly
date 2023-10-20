//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Api.UnitTests.Helpers;

internal class ShortenedUrlFactory
{
    public static List<ShortenedUrl> CreateTestEntities(
        Guid? shortUrlId = null,
        Guid? ownerId = null,
        int numEntities = 3)
    {
        var entities = new List<ShortenedUrl>();
        shortUrlId ??= Guid.NewGuid();
        ownerId ??= Guid.NewGuid();

        for (int i = 0; i < numEntities; i++)
        {
            entities.Add(CreateTestEntity(
                (i == 0) ? shortUrlId.Value : Guid.NewGuid(),
                "Test Title",
                $"https://mytest-long-address-{i}.com",
                $"Test summary for {i}",
                $"asdf42{i}",
                ownerId.Value)
            );
        }

        return entities;
    }

    public static ShortenedUrl CreateTestEntity(
        Guid shortUrlId,
        string title,
        string longUrl,
        string summary,
        string shortCode,
        Guid ownerId)
    {
        return ShortenedUrl.Hydrate(
            ShortenedUrlId.Create(shortUrlId),
            Title.Create(title),
            LongUrl.Create(longUrl),
            Summary.Create(summary),
            ShortUrlCode.Create(shortCode),
            UrlMetadata.Create(AccountId.Create(ownerId)));
    }
}
