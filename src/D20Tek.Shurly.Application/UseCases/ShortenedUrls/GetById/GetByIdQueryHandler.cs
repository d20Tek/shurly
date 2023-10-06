//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;

internal sealed class GetByIdQueryHandler : IGetByIdQueryHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public GetByIdQueryHandler(
        IShortenedUrlRepository repository,
        ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        GetByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                var result = await ShortenedUrlHelpers.GetByIdForOwner(
                    _repository,
                    query.ShortUrlId,
                    query.OwnerId);

                return result.Match<Result<ShortenedUrlResult>>(
                    entity => ShortenedUrlResult.FromEntity(entity),
                    errors => errors.ToArray());
            });
    }
}
