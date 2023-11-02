//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Api.UnitTests.Helpers;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using Microsoft.AspNetCore.Http;
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
}
