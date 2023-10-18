//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Helpers;
using D20Tek.Shurly.Domain.ShortenedUrl;
using System.Net;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class GetByShortCodeEndpointTests
{
    private readonly List<ShortenedUrl> _items;
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GetByShortCodeEndpointTests()
    {
        _items = ShortenedUrlFactory.CreateTestEntities();

        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task HandleAsync_WithValidRequest_RedirectsToLongUrl()
    {
        // arrange
        var expected = _items.First();
        await _factory.SeedDatabase(_items);

        // act
        var response = await _client.GetAsync($"/{expected.ShortUrlCode.Value}");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [TestMethod]
    public async Task HandleAsync_WithInvalidShortCode_ReturnsNotFound()
    {
        // arrange

        // act
        var response = await _client.GetAsync($"/missing-code");

        // assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void GetByShortCodeRequest_Setters()
    {
        // arrange
        var shortCode = "t3stc0de";

        var request = new GetByShortCodeRequest("");

        // act
        request = request with
        {
            shortCode = shortCode,
        };

        // assert
        request.Should().NotBeNull();
        request.shortCode.Should().Be(shortCode);
    }
}
