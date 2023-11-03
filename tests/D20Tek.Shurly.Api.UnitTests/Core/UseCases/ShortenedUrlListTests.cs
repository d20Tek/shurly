//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls;

namespace D20Tek.Shurly.Api.UnitTests.Application.UseCases;

[TestClass]
public class ShortenedUrlListTests
{
    [TestMethod]
    public void ShortenedUrlList_Setters()
    {
        // arrange
        var skip = 0;
        var take = 10;
        var total = 7;
        var shortenedUrl = new ShortenedUrlResult(
            Guid.NewGuid(),
            "Test Title",
            "https://tester-test.test.com/longurl",
            "short-url",
            "test-summary",
            new List<string> { "tag1", "tag2", "tag3" },
            1,
            DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow);

        var items = new List<ShortenedUrlResult> { shortenedUrl };

        var response = new ShortenedUrlList(
            0,
            0,
            0,
            new List<ShortenedUrlResult>());

        // act
        response = response with
        {
            Skip = skip,
            Take = take,
            TotalItems = total,
            Items = items
        };

        // assert
        response.Should().NotBeNull();
        response.Skip.Should().Be(skip);
        response.Take.Should().Be(take);
        response.TotalItems.Should().Be(total);
        response.Items.Should().BeEquivalentTo(items);
    }
}
