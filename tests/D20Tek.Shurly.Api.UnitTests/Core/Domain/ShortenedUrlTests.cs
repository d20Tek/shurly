﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Api.UnitTests.Core.Domain;

[TestClass]
public class ShortenedUrlTests
{
    [TestMethod]
    public void Create_WithValueObjects_ReturnsEntity()
    {
        // arrange
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var title = Title.Create("Test Title");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string>();
        var publishOn = DateTime.UtcNow.AddDays(10);

        // act
        var entity = ShortenedUrl.Create(
            title,
            longUrl,
            summary,
            shortCode,
            ownerId,
            tags,
            publishOn);

        // assert
        entity.Should().NotBeNull();
        entity.Id.Value.Should().NotBeEmpty();
        entity.Title.Should().Be(title);
        entity.LongUrl.Should().Be(longUrl);
        entity.Summary.Should().Be(summary);
        entity.UrlMetadata.OwnerId.Should().Be(ownerId);
        entity.UrlMetadata.PublishOn.Should().Be(publishOn);
        entity.UrlMetadata.State.Should().Be(UrlState.New);
    }

    [TestMethod]
    public void Create_WithNoPublishOn_ReturnsEntity()
    {
        // arrange
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var title = Title.Create("Test Title");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string>();

        // act
        var entity = ShortenedUrl.Create(title, longUrl, summary, shortCode, ownerId, tags, null);

        // assert
        entity.Should().NotBeNull();
        entity.Id.Value.Should().NotBeEmpty();
        entity.LongUrl.Should().Be(longUrl);
        entity.Summary.Should().Be(summary);
        entity.UrlMetadata.OwnerId.Should().Be(ownerId);
        entity.UrlMetadata.State.Should().Be(UrlState.Published);
    }

    [TestMethod]
    public void Create_WithPastPublishOn_ReturnsPublishedEntity()
    {
        // arrange
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var title = Title.Create("Test Title");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string> { "foo" };
        var publishOn = DateTime.UtcNow.AddDays(-1);

        // act
        var entity = ShortenedUrl.Create(
            title,
            longUrl,
            summary,
            shortCode,
            ownerId,
            tags,
            publishOn);

        // assert
        entity.Should().NotBeNull();
        entity.Id.Value.Should().NotBeEmpty();
        entity.LongUrl.Should().Be(longUrl);
        entity.Summary.Should().Be(summary);
        entity.UrlMetadata.OwnerId.Should().Be(ownerId);
        entity.UrlMetadata.State.Should().Be(UrlState.Published);
    }

    [TestMethod]
    public void Hydrate_WithValueObjects_ReturnsEntity()
    {
        // arrange
        var urlId = ShortenedUrlId.Create();
        var title = Title.Create("Test Title");
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string> { "foo" };
        var date = DateTime.UtcNow;
        var metadata = UrlMetadata.Hydrate(UrlState.Published, ownerId, tags, date, date, date);

        // act
        var entity = ShortenedUrl.Hydrate(urlId, title, longUrl, summary, shortCode, metadata);

        // assert
        entity.Should().NotBeNull();
        entity.Id.Should().Be(urlId);
        entity.LongUrl.Should().Be(longUrl);
        entity.Summary.Should().Be(summary);
        entity.UrlMetadata.Should().Be(metadata);
    }

    [TestMethod]
    public void ChangePublishOn_WithFutureDate_KeepsStateNew()
    {
        // arrange
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var title = Title.Create("Test Title");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string>();
        var publishOn = DateTime.UtcNow.AddDays(10);

        var entity = ShortenedUrl.Create(
            title,
            longUrl,
            summary,
            shortCode,
            ownerId,
            tags,
            publishOn);

        // act
        var newPublish = DateTime.UtcNow.AddDays(1);
        entity.ChangePublishOn(newPublish);

        // assert
        entity.Should().NotBeNull();
        entity.UrlMetadata.PublishOn.Should().Be(newPublish);
        entity.UrlMetadata.State.Should().Be(UrlState.New);
    }

    [TestMethod]
    public void ChangePublishOn_WithPastDate_PublishesUrl()
    {
        // arrange
        var longUrl = LongUrl.Create("https://testing.test.com/longurl");
        var title = Title.Create("Test Title");
        var summary = Summary.Create("test summary");
        var shortCode = ShortUrlCode.Create("foo-bar");
        var ownerId = AccountId.Create();
        var tags = new List<string>();
        var publishOn = DateTime.UtcNow.AddDays(10);

        var entity = ShortenedUrl.Create(
            title,
            longUrl,
            summary,
            shortCode,
            ownerId,
            tags,
            publishOn);

        // act
        var newPublish = DateTime.UtcNow.AddDays(-1);
        entity.ChangePublishOn(newPublish);

        // assert
        entity.Should().NotBeNull();
        entity.UrlMetadata.State.Should().Be(UrlState.Published);
    }
}
