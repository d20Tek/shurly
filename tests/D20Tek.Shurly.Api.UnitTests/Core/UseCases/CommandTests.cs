//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.PublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.UnpublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;

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

    [TestMethod]
    public void UpdateShortenedUrlCommand_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var ownerId = Guid.NewGuid();
        var publishOn = DateTime.UtcNow;

        var command = new UpdateShortenedUrlCommand(Guid.Empty, "", "", new Guid());

        // act
        command = command with
        {
            ShortUrlId = id,
            LongUrl = longUrl,
            Summary = summary,
            OwnerId = ownerId,
            PublishOn = publishOn
        };

        // assert
        command.Should().NotBeNull();
        command.ShortUrlId.Should().Be(id);
        command.LongUrl.Should().Be(longUrl);
        command.Summary.Should().Be(summary);
        command.OwnerId.Should().Be(ownerId);
        command.PublishOn.Should().Be(publishOn);
    }

    [TestMethod]
    public void DeleteShortenedUrlCommand_Setters()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var command = new DeleteShortenedUrlCommand(Guid.Empty, Guid.Empty);

        // act
        command = command with
        {
            ShortUrlId = urlId,
            OwnerId = ownerId
        };

        // assert
        command.Should().NotBeNull();
        command.ShortUrlId.Should().Be(urlId);
        command.OwnerId.Should().Be(ownerId);
    }

    [TestMethod]
    public void PublishShortenedUrlCommand_Setters()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var command = new PublishShortenedUrlCommand(Guid.Empty, Guid.Empty);

        // act
        command = command with
        {
            ShortUrlId = urlId,
            OwnerId = ownerId
        };

        // assert
        command.Should().NotBeNull();
        command.ShortUrlId.Should().Be(urlId);
        command.OwnerId.Should().Be(ownerId);
    }

    [TestMethod]
    public void UnpublishShortenedUrlCommand_Setters()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var command = new UnpublishShortenedUrlCommand(Guid.Empty, Guid.Empty);

        // act
        command = command with
        {
            ShortUrlId = urlId,
            OwnerId = ownerId
        };

        // assert
        command.Should().NotBeNull();
        command.ShortUrlId.Should().Be(urlId);
        command.OwnerId.Should().Be(ownerId);
    }
}
