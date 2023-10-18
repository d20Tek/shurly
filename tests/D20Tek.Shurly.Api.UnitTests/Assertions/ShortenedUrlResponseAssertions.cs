//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Domain.ShortenedUrl;
using System.Net.Http.Json;

namespace D20Tek.Shurly.Api.UnitTests.Assertions;

internal static class ShortenedUrlResponseAssertions
{
    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        ShortenedUrl entity)
    {
        var shortUrl = await httpResponse.Content.ReadFromJsonAsync<ShortenedUrlResponse>();

        shortUrl.Should().NotBeNull();
        shortUrl!.Id.Should().Be(entity.Id.Value.ToString());
        shortUrl.LongUrl.Should().Be(entity.LongUrl.Value);
        shortUrl.Summary.Should().Be(entity.Summary.Value);
        shortUrl.ShortUrl.Should().Be($"http://localhost/{entity.ShortUrlCode.Value}");
        shortUrl.PublishOn.Should().Be(entity.UrlMetadata.PublishOn);
        shortUrl.State.Should().Be((int)entity.UrlMetadata.State);
    }
}
