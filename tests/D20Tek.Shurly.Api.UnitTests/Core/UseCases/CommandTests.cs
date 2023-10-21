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
        var title = "Test Title";
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var ownerId = Guid.NewGuid();
        var tags = new List<string> { "tag1" };
        var publishOn = DateTime.UtcNow;

        var command = new CreateShortenedUrlCommand("", "", "", new Guid(), null);

        // act
        command = command with
        {
            Title = title,
            LongUrl = longUrl,
            Summary = summary,
            CreatorId = ownerId,
            Tags = tags,
            PublishOn = publishOn
        };

        // assert
        command.Should().NotBeNull();
        command.Title.Should().Be(title);
        command.LongUrl.Should().Be(longUrl);
        command.Summary.Should().Be(summary);
        command.CreatorId.Should().Be(ownerId);
        command.Tags.Should().BeEquivalentTo(tags);
        command.PublishOn.Should().Be(publishOn);
    }

    [TestMethod]
    public void UpdateShortenedUrlCommand_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var title = "title again";
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var ownerId = Guid.NewGuid();
        var tags = new List<string> { "foo", "bar" };
        var publishOn = DateTime.UtcNow;

        var command = new UpdateShortenedUrlCommand(Guid.Empty, "", "", "", new Guid());

        // act
        command = command with
        {
            ShortUrlId = id,
            Title = title,
            LongUrl = longUrl,
            Summary = summary,
            OwnerId = ownerId,
            Tags = tags,
            PublishOn = publishOn
        };

        // assert
        command.Should().NotBeNull();
        command.ShortUrlId.Should().Be(id);
        command.Title.Should().Be(title);
        command.LongUrl.Should().Be(longUrl);
        command.Summary.Should().Be(summary);
        command.OwnerId.Should().Be(ownerId);
        command.Tags.Should().BeEquivalentTo(tags);
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
