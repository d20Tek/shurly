//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Api.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class UpdateShortenedUrlEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsShortenedUrl()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, userId, 1);

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        var request = new UpdateShortenedUrlRequest.RequestBody(
            longUrl,
            "test summary update",
            DateTime.UtcNow.AddDays(5));

        // act
        var response = await client.PutAsJsonAsync<UpdateShortenedUrlRequest.RequestBody>(
            $"/api/v1/short-url/{urlId}",
            request);

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(longUrl, "test summary update", 0);

        var expected = await client.GetAsync($"/api/v1/short-url/{urlId}");
        response.Content.Should().BeEquivalentTo(expected.Content);
    }

    [TestMethod]
    public async Task HandleAsync_WithMissingUrlId_ReturnsNotFound()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new UpdateShortenedUrlRequest.RequestBody(longUrl, "error summary");

        // act
        var response = await client.PutAsJsonAsync<UpdateShortenedUrlRequest.RequestBody>(
            $"/api/v1/short-url/{urlId}",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task HandleAsync_WithInvalidReqest_ReturnsBadRequest()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new UpdateShortenedUrlRequest.RequestBody(longUrl, "");

        // act
        var response = await client.PutAsJsonAsync<UpdateShortenedUrlRequest.RequestBody>(
            $"/api/v1/short-url/{urlId}",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task HandleAsync_WithInvalidLongUrlFormat_ReturnsBadRequest()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new UpdateShortenedUrlRequest.RequestBody("foo-bar", "test error");

        // act
        var response = await client.PutAsJsonAsync<UpdateShortenedUrlRequest.RequestBody>(
            $"/api/v1/short-url/{urlId}",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task HandleAsync_WithDifferentOwnerId_ReturnsForbidden()
    {
        // arrange
        var urlId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";
        var items = ShortenedUrlFactory.CreateTestEntities(urlId, Guid.NewGuid(), 1);

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        var request = new UpdateShortenedUrlRequest.RequestBody(longUrl, "owner error");

        // act
        var response = await client.PutAsJsonAsync<UpdateShortenedUrlRequest.RequestBody>(
            $"/api/v1/short-url/{urlId}",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [TestMethod]
    public void UpdateShortenedUrlRequest_Setters()
    {
        // arrange
        var id = Guid.NewGuid();
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var publishOn = DateTime.UtcNow;
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();

        var body = new UpdateShortenedUrlRequest.RequestBody("", "");
        var request = new UpdateShortenedUrlRequest(new Guid(), body, null!, null!);

        // act
        body = body with
        {
            LongUrl = longUrl,
            Summary = summary,
            PublishOn = publishOn
        };

        request = request with
        {
            Id = id,
            Body = body,
            Context = context,
            User = user
        };

        // assert
        request.Should().NotBeNull();
        request.Id.Should().Be(id);
        request.Body.LongUrl.Should().Be(longUrl);
        request.Body.Summary.Should().Be(summary);
        request.Body.PublishOn.Should().Be(publishOn);
        request.Context.Should().Be(context);
        request.User.Should().Be(user);
    }
}
