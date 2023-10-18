//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

namespace D20Tek.Shurly.Api.UnitTests.Endpoints.Home;

[TestClass]
public class HomeEndpointsTests
{
    private readonly string userId = Guid.NewGuid().ToString();

    [TestMethod]
    public async Task Home_ReturnsTitle()
    {
        // arrange
        var factory = new TestWebApplicationFactory();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync("/");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        var text = await response.Content.ReadAsStringAsync();
        text.Should().Be("Shurly Api - Link shortening and management");
    }

    [TestMethod]
    public async Task AuthenticatedUser_ReturnsMessage()
    {
        // arrange
        var factory = new TestWebApplicationFactory();
        var client = factory.CreateAuthenticatedClient();

        // act
        var response = await client.GetAsync("/user");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        var text = await response.Content.ReadAsStringAsync();
        text.Should().Be("Authenticated user accessed this api.");
    }

    [TestMethod]
    public async Task AuthenticatedAdmin_ReturnsMessage()
    {
        // arrange
        var factory = new TestWebApplicationFactory();
        var token = factory.GenerateTestAccessToken(userId, roles: new string[] { "admin" });
        var client = factory.CreateAuthenticatedClient(token);

        // act
        var response = await client.GetAsync("/admin");

        // assert
        response.Should().NotBeNull();
        response.IsSuccessStatusCode.Should().BeTrue();
        var text = await response.Content.ReadAsStringAsync();
        text.Should().Be("Authenticated admin accessed this api.");
    }
}
