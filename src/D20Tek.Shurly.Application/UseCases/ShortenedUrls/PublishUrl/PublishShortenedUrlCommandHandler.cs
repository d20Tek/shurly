//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.PublishUrl;

internal class PublishShortenedUrlCommandHandler : IPublishShortenedUrlCommandHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public PublishShortenedUrlCommandHandler(
        IShortenedUrlRepository repository,
        ILogger<PublishShortenedUrlCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        PublishShortenedUrlCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                // 1. get existing shortened url entity own by current user
                var result = await ShortenedUrlHelpers.GetByIdForOwner(
                    _repository,
                    command.ShortUrlId,
                    command.OwnerId);

                return await result.MatchAsync<Result<ShortenedUrlResult>>(
                    async entity =>
                    {
                        // 2. update entity's publish state
                        entity.UrlMetadata.PublishUrl();

                        // 3. persist the entity change
                        var created = await _repository.UpdateAsync(entity);
                        if (created is false)
                        {
                            return DomainErrors.UpdateFailed;
                        }

                        return ShortenedUrlResult.FromEntity(entity);
                    },
                    errors => Task.FromResult<Result<ShortenedUrlResult>>(errors.ToArray()));
            });
    }
}
