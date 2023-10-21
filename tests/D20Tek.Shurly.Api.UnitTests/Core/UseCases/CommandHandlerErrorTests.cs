//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Api.UnitTests.Assertions;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.PublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.UnpublishUrl;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Api.UnitTests.Application.UseCases;

[TestClass]
public class CommandHandlerErrorTests
{
    [TestMethod]
    public async Task CreateHandleAsync_WithRepoError_ReturnsErrorResult()
    {
        // arrange
        var repo = new Mock<IShortenedUrlRepository>().Object;
        var validator = new CreateShortenedUrlCommandValidator();
        var logger = new Mock<ILogger<CreateShortenedUrlCommandHandler>>().Object;
        var urlShorteningService = new Mock<IUrlShorteningService>().Object;

        var handler = new CreateShortenedUrlCommandHandler(
            repo,
            urlShorteningService,
            validator,
            logger);

        var command = new CreateShortenedUrlCommand(
            "test title",
            "https://test.com/longurl",
            "test summary",
            Guid.NewGuid());

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(DomainErrors.CreateFailed);
    }

    [TestMethod]
    public async Task DeleteHandleAsync_WithRepoError_ReturnsErrorResult()
    {
        // arrange
        var ownerId = Guid.NewGuid();
        var logger = new Mock<ILogger<DeleteShortenedUrlCommandHandler>>().Object;
        var repo = CreateMockRepo(ownerId);

        var handler = new DeleteShortenedUrlCommandHandler(repo.Object, logger);
        var command = new DeleteShortenedUrlCommand(Guid.NewGuid(), ownerId);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(DomainErrors.DeleteFailed);
    }

    [TestMethod]
    public async Task UpdateHandleAsync_WithRepoError_ReturnsErrorResult()
    {
        // arrange
        var ownerId = Guid.NewGuid();
        var logger = new Mock<ILogger<UpdateShortenedUrlCommandHandler>>().Object;
        var validator = new UpdateShortenedUrlCommandValidator();
        var repo = CreateMockRepo(ownerId);

        var handler = new UpdateShortenedUrlCommandHandler(repo.Object, validator, logger);
        var command = new UpdateShortenedUrlCommand(
            Guid.NewGuid(),
            "updated",
            "https://test.com/longurl",
            "test summary",
            ownerId);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(DomainErrors.UpdateFailed);
    }

    [TestMethod]
    public async Task PublishHandleAsync_WithRepoError_ReturnsErrorResult()
    {
        // arrange
        var ownerId = Guid.NewGuid();
        var logger = new Mock<ILogger<PublishShortenedUrlCommandHandler>>().Object;
        var repo = CreateMockRepo(ownerId);

        var handler = new PublishShortenedUrlCommandHandler(repo.Object, logger);
        var command = new PublishShortenedUrlCommand(Guid.NewGuid(), ownerId);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(DomainErrors.UpdateFailed);
    }

    [TestMethod]
    public async Task UnpublishHandleAsync_WithRepoError_ReturnsErrorResult()
    {
        // arrange
        var ownerId = Guid.NewGuid();
        var logger = new Mock<ILogger<UnpublishShortenedUrlCommandHandler>>().Object;
        var repo = CreateMockRepo(ownerId);

        var handler = new UnpublishShortenedUrlCommandHandler(repo.Object, logger);
        var command = new UnpublishShortenedUrlCommand(Guid.NewGuid(), ownerId);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(DomainErrors.UpdateFailed);
    }

    private Mock<IShortenedUrlRepository> CreateMockRepo(Guid ownerId)
    {
        var repo = new Mock<IShortenedUrlRepository>();
        repo.Setup(x => x.GetByIdAsync(It.IsAny<ShortenedUrlId>()))
            .ReturnsAsync(ShortenedUrl.Create(
                Title.Create("Test Title"),
                LongUrl.Create("https://longurl.test.com"),
                Summary.Create("test summary"),
                ShortUrlCode.Create("foo-bar1"),
                AccountId.Create(ownerId),
                new List<string>(),
                null));

        return repo;
    }
}
