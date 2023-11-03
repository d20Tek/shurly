//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class ShortenedUrlListResponseTests
{
    [TestMethod]
    public void ShortenedUrlListResponse_Setters()
    {
        // arrange
        var shortUrl = new ShortenedUrlResponse(
            "test-url-id",
            "test title",
            "https://tester-test.test.com/longurl",
            "short-url",
            "test-summary",
            new List<string> { "tag" },
            1,
            DateTime.UtcNow.AddHours(-3),
            DateTime.Now);

        var metadata = new ListMetadata(7, 7, 0, 25);
        var links = new List<LinkMetadata>
        {
            new LinkMetadata("test-type", "test-url"),
            new LinkMetadata("test-2", "foo-bar")
        };
        var items = new List<ShortenedUrlResponse> { shortUrl };

        var response = new ShortenedUrlListResponse(
            new ListMetadata(0, 0, 0, 0),
            new List<LinkMetadata>(),
            new List<ShortenedUrlResponse>());

        // act
        response = response with
        {
            Metadata = metadata,
            Links = links,
            Items = items,
        };

        // assert
        response.Should().NotBeNull();
        response.Metadata.Should().Be(metadata);
        response.Links.Should().BeEquivalentTo(links);
        response.Items.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void ListMetadata_Setter()
    {
        // arrange
        var total = 8;
        var itemCt = 2;
        var skip = 6;
        var take = 3;

        var metadata = new ListMetadata(0, 0, 0, 0);

        // act
        metadata = metadata with
        {
            TotalItems = total,
            ItemsCount = itemCt,
            Skip = skip,
            Take = take
        };

        // assert
        metadata.Should().NotBeNull();
        metadata.TotalItems.Should().Be(total);
        metadata.ItemsCount.Should().Be(itemCt);
        metadata.Skip.Should().Be(skip);
        metadata.Take.Should().Be(take);
    }

    [TestMethod]
    public void LinkMetadata_Setters()
    {
        // arrange
        var type = "test-type";
        var url = "https://test.com/test12345";

        var link = new LinkMetadata("", "");

        // act
        link = link with
        {
            Type = type,
            Url = url,
        };

        // assert
        link.Should().NotBeNull();
        link.Type.Should().Be(type);
        link.Url.Should().Be(url);
    }
}
