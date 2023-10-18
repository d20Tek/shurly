//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class CreateShortenedUrlEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsShortenedUrl()
    {
        // arrange
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new CreateShortenedUrlRequest(longUrl, "test summary");

        // act
        var response = await client.PostAsJsonAsync<CreateShortenedUrlRequest>(
            $"/api/v1/short-url",
            request);

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(longUrl, "test summary", 1);
    }

    [TestMethod]
    public async Task HandleAsync_WithValidFutureRequest_ReturnsShortenedUrl()
    {
        // arrange
        var userId = Guid.NewGuid();
        var longUrl = "https://tester-test.test.com/longurl";

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new CreateShortenedUrlRequest(
            longUrl,
            "test summary",
            DateTime.UtcNow.AddDays(5));

        // act
        var response = await client.PostAsJsonAsync<CreateShortenedUrlRequest>(
            $"/api/v1/short-url",
            request);

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(longUrl, "test summary", 0);
    }

    [TestMethod]
    public async Task HandleAsync_WithInvalidRequest_ReturnsBadRequest()
    {
        // arrange
        var userId = Guid.NewGuid();

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new CreateShortenedUrlRequest(
            "",
            "",
            DateTime.UtcNow.AddDays(5));

        // act
        var response = await client.PostAsJsonAsync<CreateShortenedUrlRequest>(
            $"/api/v1/short-url",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task HandleAsync_WithLongUrlFormat_ReturnsBadRequest()
    {
        // arrange
        var userId = Guid.NewGuid();

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);

        var request = new CreateShortenedUrlRequest(
            "foo-bar",
            "test summary",
            DateTime.UtcNow.AddDays(5));

        // act
        var response = await client.PostAsJsonAsync<CreateShortenedUrlRequest>(
            $"/api/v1/short-url",
            request);

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public void CreateShortenedUrlRequest_Setters()
    {
        // arrange
        var longUrl = "https://tester.test.com";
        var summary = "test summary";
        var publishOn = DateTime.UtcNow;

        var command = new CreateShortenedUrlRequest("", "");

        // act
        command = command with
        {
            LongUrl = longUrl,
            Summary = summary,
            PublishOn = publishOn
        };

        // assert
        command.Should().NotBeNull();
        command.LongUrl.Should().Be(longUrl);
        command.Summary.Should().Be(summary);
        command.PublishOn.Should().Be(publishOn);
    }
}
