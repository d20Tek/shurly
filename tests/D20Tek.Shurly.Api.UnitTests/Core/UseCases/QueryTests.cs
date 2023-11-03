//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;

namespace D20Tek.Shurly.Api.UnitTests.Application.UseCases;

[TestClass]
public class QueryTests
{
    [TestMethod]
    public void GetByIdQuery_Setters()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var query = new GetByIdQuery(Guid.Empty, Guid.Empty);

        // act
        query = query with
        {
            ShortUrlId = urlId,
            OwnerId = ownerId,
        };

        // assert
        query.Should().NotBeNull();
        query.ShortUrlId.Should().Be(urlId);
        query.OwnerId.Should().Be(ownerId);
    }

    [TestMethod]
    public void GetByOwnerQuery_Setters()
    {
        // arrange
        var ownerId = Guid.NewGuid();
        var skip = 100;
        var take = 5;

        var query = new GetByOwnerQuery(Guid.Empty, 0, 10);

        // act
        query = query with
        {
            OwnerId = ownerId,
            Skip = skip,
            Take = take
        };

        // assert
        query.Should().NotBeNull();
        query.OwnerId.Should().Be(ownerId);
        query.Skip.Should().Be(skip);
        query.Take.Should().Be(take);
    }

    [TestMethod]
    public void GetByShortCodeQuery_Setters()
    {
        // arrange
        var shortCode = "t3stC0D3";

        var query = new GetByShortCodeQuery("");

        // act
        query = query with
        {
            ShortUrlCode = shortCode,
        };

        // assert
        query.Should().NotBeNull();
        query.ShortUrlCode.Should().Be(shortCode);
    }
}
