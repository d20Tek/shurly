//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.ShortenedUrl;
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

    public async Task<Result<ShortenedUrlList>> HandleAsync(
        GetByOwnerQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlList>(
            _logger,
            async () =>
            {
                var ownerId = AccountId.Create(query.OwnerId);

                var entities = await _repository.GetForOwnerAsync(ownerId, query.Skip, query.Take);
                var results = entities.Select(x => ShortenedUrlResult.FromEntity(x))
                                      .ToList();

                var totalCount = await _repository.GetCountForOwnerAsync(ownerId);
                return new ShortenedUrlList(query.Skip, query.Take, totalCount, results);
            });
    }
}
