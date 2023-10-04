//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;

internal class GetByOwnerQueryHandler : IGetByOwnerQueryHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public GetByOwnerQueryHandler(
        IShortenedUrlRepository repository,
        ILogger<GetByOwnerQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ShortenedUrlResult>>> HandleAsync(
        GetByOwnerQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<IEnumerable<ShortenedUrlResult>>(
            _logger,
            async () =>
            {
                var ownerId = AccountId.Create(query.OwnerId);
                var entities = await _repository.GetForOwnerAsync(ownerId);

                var results = entities.Select(x => ShortenedUrlResult.FromEntity(x))
                                      .ToList();
                return results;
            });
    }
}
