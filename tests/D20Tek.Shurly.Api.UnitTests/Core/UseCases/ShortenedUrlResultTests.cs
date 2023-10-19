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
        var longUrl = "https://tester-test.test.com/longurl";
        var shortUrl = "short-url";
        var summary = "test-summary";
        var state = 1;
        var publishOn = DateTime.UtcNow;

        var response = new ShortenedUrlResult(Guid.Empty, "", "", "", 0, DateTime.Now);

        // act
        response = response with
        {
            Id = id,
            LongUrl = longUrl,
            ShortUrlCode = shortUrl,
            Summary = summary,
            State = state,
            PublishOn = publishOn
        };

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(id);
        response.LongUrl.Should().Be(longUrl);
        response.ShortUrlCode.Should().Be(shortUrl);
        response.Summary.Should().Be(summary);
        response.State.Should().Be(state);
        response.PublishOn.Should().Be(publishOn);
    }
}
