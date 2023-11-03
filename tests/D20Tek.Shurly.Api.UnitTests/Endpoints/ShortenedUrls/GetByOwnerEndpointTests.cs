//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Api.UnitTests.Helpers;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.ShortenedUrls;

[TestClass]
public class GetByOwnerEndpointTests
{
    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsShortenedUrlList()
    {
        // arrange
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(ownerId: userId);

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(
            items.OrderByDescending(x => x.UrlMetadata.CreatedOn).ToList());
    }

    [TestMethod]
    public async Task HandleAsync_WithQueryHandlerError_ReturnsProblem()
    {
        // arrange
        var request = new GetByOwnerRequest(new DefaultHttpContext(), new ClaimsPrincipal());
        var handler = new Mock<IGetByOwnerQueryHandler>();
        handler.Setup(x => x.HandleAsync(It.IsAny<GetByOwnerQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(Error.Unexpected("test.error", "mocked call..."));

        var endpoint = new GetByOwnerEndpoint();

        // act
        var result = await endpoint.HandleAsync(request, handler.Object, CancellationToken.None);

        // assert
        result.ShouldBeProblemDetails(StatusCodes.Status500InternalServerError);
    }

    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsFirstPagedList()
    {
        // arrange
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(ownerId: userId);
        var expectedLinks = new List<LinkMetadata>
        { 
            new LinkMetadata("firstLink", "first"),
            new LinkMetadata("lastLink", "last"),
            new LinkMetadata("nextLink", "next")
        };

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url?skip=0&take=1");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(
            items.OrderByDescending(x => x.UrlMetadata.CreatedOn).Take(1).ToList(),
            0,
            1,
            3,
            expectedLinks);
    }

    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsNextPagedList()
    {
        // arrange
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(ownerId: userId);
        var expectedLinks = new List<LinkMetadata>
        {
            new LinkMetadata("firstLink", "first"),
            new LinkMetadata("lastLink", "last"),
            new LinkMetadata("nextLink", "next"),
            new LinkMetadata("prevLink", "prev")
        };

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url?skip=1&take=1");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(
            items.OrderByDescending(x => x.UrlMetadata.CreatedOn).Skip(1).Take(1).ToList(),
            1,
            1,
            3,
            expectedLinks);
    }

    [TestMethod]
    public async Task HandleAsync_WithValidRequest_ReturnsLastPagedList()
    {
        // arrange
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(ownerId: userId);
        var expectedLinks = new List<LinkMetadata>
        {
            new LinkMetadata("firstLink", "first"),
            new LinkMetadata("lastLink", "last"),
            new LinkMetadata("prevLink", "prev")
        };

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url?skip=2&take=1");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(
            items.OrderByDescending(x => x.UrlMetadata.CreatedOn).Skip(2).Take(1).ToList(),
            2,
            1,
            3,
            expectedLinks);
    }

    [TestMethod]
    public async Task HandleAsync_WithSkipPastEnd_ReturnsLastPagedList()
    {
        // arrange
        var userId = Guid.NewGuid();
        var items = ShortenedUrlFactory.CreateTestEntities(ownerId: userId);
        var expectedLinks = new List<LinkMetadata>
        {
            new LinkMetadata("firstLink", "first"),
            new LinkMetadata("lastLink", "last"),
            new LinkMetadata("prevLink", "prev")
        };

        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId.ToString());
        var client = factory.CreateAuthenticatedClient(token);
        await factory.SeedDatabase(items);

        // act
        var response = await client.GetAsync($"/api/v1/short-url?skip=100&take=10");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        await response.ShouldBeEquivalentTo(
            new List<ShortenedUrl>(),
            100,
            10,
            3,
            expectedLinks);
    }

    [TestMethod]
    public void GetByOwnerRequest_Setters()
    {
        // arrange
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();
        var skip = 5;
        var take = 10;

        var request = new GetByOwnerRequest(null!, null!);

        // act
        request = request with
        {
            Context = context,
            User = user,
            Skip = skip,
            Take = take
        };

        // assert
        request.Should().NotBeNull();
        request.Context.Should().Be(context);
        request.User.Should().Be(user);
        request.Skip.Should().Be(skip);
        request.Take.Should().Be(take);
    }
}
