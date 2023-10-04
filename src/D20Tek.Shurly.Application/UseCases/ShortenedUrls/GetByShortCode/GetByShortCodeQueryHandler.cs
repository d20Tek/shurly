//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;
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
                var code = ShortUrlCode.Create(query.ShortUrlCode);
                var entity = await _repository.GetByShortUrlCodeAsync(code);
                if (entity is null)
                {
                    return DomainErrors.ShortUrlNotFound;
                }

                return ShortenedUrlResult.FromEntity(entity);
            });
    }
}
