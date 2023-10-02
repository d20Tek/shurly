//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;
using D20Tek.Shurly.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;

internal sealed class GetByIdQueryHandler : IGetByIdQueryHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public GetByIdQueryHandler(
        IShortenedUrlRepository repository,
        ILogger<GetByShortCodeQueryHandler> logger)
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
                var id = ShortenedUrlId.Create(query.Id);
                var entity = await _repository.GetByIdAsync(id);
                if (entity is null)
                {
                    return DomainErrors.EntityNotFound("ShortenedUrl", query.Id);
                }

                return ShortenedUrlResult.FromEntity(entity);
            });
    }
}
