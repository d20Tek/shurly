//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;

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

        var query = new GetByOwnerQuery(Guid.Empty);

        // act
        query = query with
        {
            OwnerId = ownerId,
        };

        // assert
        query.Should().NotBeNull();
        query.OwnerId.Should().Be(ownerId);
    }
}
