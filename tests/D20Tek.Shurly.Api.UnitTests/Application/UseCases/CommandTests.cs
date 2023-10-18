//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;

namespace D20Tek.Shurly.Api.UnitTests.Application.UseCases;

[TestClass]
public class CommandTests
{
    [TestMethod]
    public void CreateShortenedUrlCommand_Setters()
    {
        // arrange
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var ownerId = Guid.NewGuid();
        var publishOn = DateTime.UtcNow;

        var command = new CreateShortenedUrlCommand("", "", new Guid());

        // act
        command = command with
        {
            LongUrl = longUrl,
            Summary = summary,
            CreatorId = ownerId,
            PublishOn = publishOn
        };

        // assert
        command.Should().NotBeNull();
        command.LongUrl.Should().Be(longUrl);
        command.Summary.Should().Be(summary);
        command.CreatorId.Should().Be(ownerId);
        command.PublishOn.Should().Be(publishOn);
    }
}
