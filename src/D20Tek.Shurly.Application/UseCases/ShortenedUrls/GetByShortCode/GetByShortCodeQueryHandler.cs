﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;
using D20Tek.Shurly.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;

internal class GetByShortCodeQueryHandler : IGetByShortCodeQueryHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public GetByShortCodeQueryHandler(
        IShortenedUrlRepository repository,
        ILogger<GetByShortCodeQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        GetByShortCodeQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                var entity = await _repository.GetByShortUrlCodeAsync(query.ShortUrlCode);
                if (entity is null)
                {
                    return DomainErrors.ShortUrlNotFound;
                }

                return MapResult(entity);
            });
    }

    private Result<ShortenedUrlResult> MapResult(ShortenedUrl entity)
    {
        return new ShortenedUrlResult(
            entity.Id.Value,
            entity.LongUrl.Value,
            entity.ShortUrlCode.Value,
            entity.Summary.Value,
            (int)entity.UrlMetadata.State,
            entity.UrlMetadata.PublishOn);
    }
}
