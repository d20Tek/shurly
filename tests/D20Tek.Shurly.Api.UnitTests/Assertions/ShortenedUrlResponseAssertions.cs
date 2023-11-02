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
        shortUrl.Tags.Should().BeEquivalentTo(entity.UrlMetadata.Tags);
        shortUrl.PublishOn.Should().Be(entity.UrlMetadata.PublishOn);
        shortUrl.State.Should().Be((int)entity.UrlMetadata.State);
    }

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        List<ShortenedUrl> entities)
    {
        var list = await httpResponse.Content.ReadFromJsonAsync<ShortenedUrlListResponse>();

        list.Should().NotBeNull();
        list!.Metadata.TotalItems.Should().Be(entities.Count());
        list.Metadata.Skip.Should().Be(0);
        list.Metadata.Take.Should().Be(25);

        list.Links.Should().HaveCount(2);
        list.Links.Should().Contain(x => x.Type == "firstLink");
        list.Links.Should().Contain(x => x.Type == "lastLink");

        for (int i = 0; i < list.Metadata.TotalItems; i++)
        {
            list.Items[i].Id.Should().Be(entities[i].Id.Value.ToString());
            list.Items[i].Title.Should().Be(entities[i].Title.Value);
            list.Items[i].LongUrl.Should().Be(entities[i].LongUrl.Value);
            list.Items[i].Summary.Should().Be(entities[i].Summary.Value);
            list.Items[i].ShortUrl.Should().Be($"http://localhost/{entities[i].ShortUrlCode.Value}");
            list.Items[i].Tags.Should().BeEquivalentTo(entities[i].UrlMetadata.Tags);
            list.Items[i].PublishOn.Should().Be(entities[i].UrlMetadata.PublishOn);
            list.Items[i].State.Should().Be((int)entities[i].UrlMetadata.State);
        }
    }

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        string title,
        string longUrl,
        string summary,
        int state = 0)
    {
        var shortUrl = await httpResponse.Content.ReadFromJsonAsync<ShortenedUrlResponse>();

        shortUrl.Should().NotBeNull();
        shortUrl!.Id.Should().NotBeNull();
        shortUrl.Title.Should().Be(title);
        shortUrl.LongUrl.Should().Be(longUrl);
        shortUrl.Summary.Should().Be(summary);
        shortUrl.ShortUrl.Should().NotBeNull();
        shortUrl.Tags.Should().BeEmpty();
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
