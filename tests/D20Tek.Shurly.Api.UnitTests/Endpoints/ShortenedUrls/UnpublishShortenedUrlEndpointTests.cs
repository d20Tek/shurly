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
public class UnpublishShortenedUrlEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsShortenedUrl()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, userId);

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.PutAsync($"/api/v1/short-url/{urlId}/unpublish", null);

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBePublishedState(urlId.ToString(), 2);
    }

    [TestMethod]
    public async Task HandleAsync_WithMissingUrlId_ReturnsNotFound()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        // act
        var response = await client.PutAsync($"/api/v1/short-url/{urlId}/unpublish", null);

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
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, userId);

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(Guid.NewGuid().ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.PutAsync($"/api/v1/short-url/{urlId}/unpublish", null);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TestMethod]
    public void UnpublishShortenedUrlRequest_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();

        var request = new UnpublishShortenedUrlRequest(Guid.Empty, null!, null!);

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
