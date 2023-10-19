//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Shurly.Api.UnitTests.Core;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void RetrieveDbContext_Succeeds()
    {
        // arrange
        var config = CreateMockConfiguration(true);
        var services = new ServiceCollection();
        services.AddInfrastructureServices(config.Object);

        var provider = services.BuildServiceProvider();

        // act
        using var scope = provider.CreateScope();
        var result = scope.ServiceProvider.GetRequiredService<ShurlyDbContext>();

        // assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DbContext_WithMissingConnectionString_Throws()
    {
        // arrange
        var config = CreateMockConfiguration(false);
        var services = new ServiceCollection();

        // act
        services.AddInfrastructureServices(config.Object);

        // assert
    }

    private static Mock<IConfiguration> CreateMockConfiguration(bool includeConfigSection)
    {
        var mockSection = new Mock<IConfigurationSection>();
        if (includeConfigSection)
        {
            mockSection.Setup(x => x["ShurlyConnection"])
                       .Returns("Server=(localdb)\\mssqllocaldb;Database=d20tek-shurly-db;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        var config = new Mock<IConfiguration>();
        config.Setup(x => x.GetSection(It.IsAny<string>()))
              .Returns(mockSection.Object);

        return config;
    }
}
