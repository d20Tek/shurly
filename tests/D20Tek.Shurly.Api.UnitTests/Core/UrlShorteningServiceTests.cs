//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Shurly.Api.UnitTests.Core;

[TestClass]
public class UrlShorteningServiceTests
{
    [TestMethod]
    public async Task GenerateUniqueCodeAsync_WithNoSeed_ReturnsCode()
    {
        // arrange
        var repo = new Mock<IShortenedUrlRepository>();
        repo.Setup(x => x.IsUrlCodeUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var service = new UrlShorteningService(repo.Object);

        // act
        var code = await service.GenerateUniqueCodeAsync();

        // assert
        code.Should().NotBeNull();
        code.Should().HaveLength(8);
    }

    [TestMethod]
    public async Task GenerateUniqueCodeAsync_WithGuidSeed_ReturnsCode()
    {
        // arrange
        var repo = new Mock<IShortenedUrlRepository>();
        repo.Setup(x => x.IsUrlCodeUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var service = new UrlShorteningService(repo.Object);

        // act
        var code = await service.GenerateUniqueCodeAsync(Guid.NewGuid());

        // assert
        code.Should().NotBeNull();
        code.Should().HaveLength(8);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public async Task GenerateUniqueCodeAsync_WithNeverUniqueCode_Throws()
    {
        // arrange
        var repo = new Mock<IShortenedUrlRepository>().Object;
        var service = new UrlShorteningService(repo);

        // act
        var code = await service.GenerateUniqueCodeAsync();

        // assert
        code.Should().NotBeNull();
        code.Should().HaveLength(8);
    }
}
