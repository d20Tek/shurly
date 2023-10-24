//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls;

namespace D20Tek.Shurly.Api.UnitTests.Application.UseCases;

[TestClass]
public class ShortenedUrlResultTests
{
    [TestMethod]
    public void ShortenedUrlResult_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var title = "Test Title";
        var longUrl = "https://tester-test.test.com/longurl";
        var shortUrl = "short-url";
        var summary = "test-summary";
        var tags = new List<string> { "tag1", "tag2", "tag3" };
        var state = 1;
        var createdOn = DateTime.UtcNow.AddHours(-1);
        var publishOn = DateTime.UtcNow;

        var response = new ShortenedUrlResult(
            Guid.Empty,
            "",
            "",
            "",
            "",
            new List<string>(),
            0,
            DateTime.Now,
            DateTime.Now);

        // act
        response = response with
        {
            Id = id,
            Title = title,
            LongUrl = longUrl,
            ShortUrlCode = shortUrl,
            Summary = summary,
            Tags = tags,
            State = state,
            CreatedOn = createdOn,
            PublishOn = publishOn
        };

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(id);
        response.Title.Should().Be(title);
        response.LongUrl.Should().Be(longUrl);
        response.ShortUrlCode.Should().Be(shortUrl);
        response.Summary.Should().Be(summary);
        response.Tags.Should().BeEquivalentTo(tags);
        response.State.Should().Be(state);
        response.CreatedOn.Should().Be(createdOn);
        response.PublishOn.Should().Be(publishOn);
    }
}
