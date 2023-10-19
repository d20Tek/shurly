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

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        List<ShortenedUrl> entities)
    {
        var list = await httpResponse.Content.ReadFromJsonAsync<List<ShortenedUrlResponse>>();

        list.Should().NotBeNull();
        list!.Count().Should().Be(entities.Count());
        for (int i = 0; i < list!.Count(); i++)
        {
            list![i].Id.Should().Be(entities[i].Id.Value.ToString());
            list[i].LongUrl.Should().Be(entities[i].LongUrl.Value);
            list[i].Summary.Should().Be(entities[i].Summary.Value);
            list[i].ShortUrl.Should().Be($"http://localhost/{entities[i].ShortUrlCode.Value}");
            list[i].PublishOn.Should().Be(entities[i].UrlMetadata.PublishOn);
            list[i].State.Should().Be((int)entities[i].UrlMetadata.State);
        }
    }

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        string longUrl,
        string summary,
        int state = 0)
    {
        var shortUrl = await httpResponse.Content.ReadFromJsonAsync<ShortenedUrlResponse>();

        shortUrl.Should().NotBeNull();
        shortUrl!.Id.Should().NotBeNull();
        shortUrl.LongUrl.Should().Be(longUrl);
        shortUrl.Summary.Should().Be(summary);
        shortUrl.ShortUrl.Should().NotBeNull();
        shortUrl.State.Should().Be(state);
    }

    public static async Task ShouldBePublishedState(
        this HttpResponseMessage httpResponse,
        string urlId,
        int state)
    {
        var shortUrl = await httpResponse.Content.ReadFromJsonAsync<ShortenedUrlResponse>();

        shortUrl.Should().NotBeNull();
        shortUrl!.Id.Should().Be(urlId);
        shortUrl.State.Should().Be(state);
    }
}
