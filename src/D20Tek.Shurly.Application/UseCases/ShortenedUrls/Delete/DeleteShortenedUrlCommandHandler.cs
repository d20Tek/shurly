//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;

internal class DeleteShortenedUrlCommandHandler : IDeleteShortenedUrlCommandHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly ILogger _logger;

    public DeleteShortenedUrlCommandHandler(
        IShortenedUrlRepository repository,
        ILogger<DeleteShortenedUrlCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        DeleteShortenedUrlCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                var result = await ShortenedUrlHelpers.GetByIdForOwner(
                    _repository,
                    command.ShortUrlId,
                    command.OwnerId);

                if (result.IsFailure) return result.ErrorsList;

                var entity = result.Value;
                if (await _repository.DeleteAsync(entity) is false)
                {
                    return DomainErrors.DeleteFailed;
                }

                return ShortenedUrlResult.FromEntity(entity);
            });
    }
}
