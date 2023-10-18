//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class GetByIdEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsShortenedUrl()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var items = new List<ShortenedUrl>
        {
            ShortenedUrl.Hydrate(
                ShortenedUrlId.Create(urlId),
                LongUrl.Create("https://mytest-long-address.com"),
                Summary.Create("Test summary"),
                ShortUrlCode.Create("asdf1234"),
                UrlMetadata.Create(AccountId.Create(userId)))
        };
        
        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url/{urlId}");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(items.First());
    }

    [TestMethod]
    public async Task HandleAsync_WithMissingUrlId_ReturnsNotFound()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var factory = new TestWebApplicationFactory();
        var client = factory.CreateAuthenticatedClient();

        // act
        var response = await client.GetAsync($"/api/v1/short-url/{urlId}");

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task HandleAsync_WithOtherUsersId_ReturnsForbidden()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var items = new List<ShortenedUrl>
        {
            ShortenedUrl.Hydrate(
                ShortenedUrlId.Create(urlId),
                LongUrl.Create("https://mytest-long-address.com"),
                Summary.Create("Test summary"),
                ShortUrlCode.Create("asdf1234"),
                UrlMetadata.Create(AccountId.Create(userId)))
        };

        var factory = new TestWebApplicationFactory();
        var client = factory.CreateAuthenticatedClient();
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url/{urlId}");

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TestMethod]
    public void GetByIdRequest_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();

        var request = new GetByIdRequest(Guid.Empty, null!, null!);

        // act
        request = request with
        {
            Id = id,
            Context = context,
            User = user,
        };

        // assert
        request.Should().NotBeNull();
        request.Id.Should().Be(id);
        request.Context.Should().Be(context);
        request.User.Should().Be(user);
    }

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
    public void ShortenedUrlResponse_Setters()
    {
        // arrange
        var id = "test-url-id";
        var longUrl = "https://tester-test.test.com/longurl";
        var shortUrl = "short-url";
        var summary = "test-summary";
        var state = 1;
        var publishOn = DateTime.UtcNow;

        var response = new ShortenedUrlResponse("", "", "", "", 0, DateTime.Now);

        // act
        response = response with
        {
            Id = id,
            LongUrl = longUrl,
            ShortUrl = shortUrl,
            Summary = summary,
            State = state,
            PublishOn = publishOn
        };

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(id);
        response.LongUrl.Should().Be(longUrl);
        response.ShortUrl.Should().Be(shortUrl);
        response.Summary.Should().Be(summary);
        response.State.Should().Be(state);
        response.PublishOn.Should().Be(publishOn);
    }
}
