//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Api.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class DeleteShortenedUrlEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsDeletedUrlId()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, userId);
        var expected = items.First();

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.DeleteAsync($"/api/v1/short-url/{urlId}");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(expected);

        var retrieval = await client.GetAsync($"/api/v1/short-url/{urlId}");
        retrieval.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task HandleAsync_WithMissingUrlId_ReturnsNotFound()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var factory = new TestWebApplicationFactory();
        var client = factory.CreateAuthenticatedClient();

        // act
        var response = await client.DeleteAsync($"/api/v1/short-url/{urlId}");

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
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, userId, 1);

        var factory = new TestWebApplicationFactory();
        var client = factory.CreateAuthenticatedClient();
        await factory.SeedDatabase(items);

        // act
        var response = await client.DeleteAsync($"/api/v1/short-url/{urlId}");

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TestMethod]
    public void DeleteShortenedUrlRequest_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();

        var request = new DeleteShortenedUrlRequest(Guid.Empty, null!, null!);

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
}
