﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;
using D20Tek.Shurly.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;

internal class UpdateShortenedUrlCommandHandler : IUpdateShortenedUrlCommandHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly UpdateShortenedUrlCommandValidator _validator;
    private readonly ILogger _logger;

    public UpdateShortenedUrlCommandHandler(
        IShortenedUrlRepository repository,
        UpdateShortenedUrlCommandValidator validator,
        ILogger<UpdateShortenedUrlCommandHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        UpdateShortenedUrlCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var guardResult = await ValidateGuardConditions(command);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. merge updated shortened url entity
                var existingEntity = guardResult.Value;
                var entity = MergeUpdatedEntity(existingEntity, command);

                // 3. persist the new entity
                var created = await _repository.UpdateAsync(entity);
                if (created is false)
                {
                    return DomainErrors.UpdateFailed;
                }

                return ShortenedUrlResult.FromEntity(entity);
            });
    }

    private async Task<Result<ShortenedUrl>> ValidateGuardConditions(
        UpdateShortenedUrlCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<ShortenedUrl>();
        }

        // 2. check if url string is valid
        if (!Uri.TryCreate(command.LongUrl, UriKind.Absolute, out _))
        {
            return DomainErrors.LongUrlInvalidFormat;
        }

        // 3. get the existing ShortenedUrl by id
        var id = ShortenedUrlId.Create(command.ShortUrlId);
        var existingUrl = await _repository.GetByIdAsync(id);
        if (existingUrl is null)
        {
            return DomainErrors.EntityNotFound(nameof(ShortenedUrl), command.ShortUrlId);
        }

        return existingUrl;
    }

    private ShortenedUrl MergeUpdatedEntity(
        ShortenedUrl existingEntity,
        UpdateShortenedUrlCommand command)
    {
        if (command.PublishOn is not null)
        {
            existingEntity.UrlMetadata.UpdatePublishOn(command.PublishOn.Value);
        }
        else
        {
            existingEntity.UrlMetadata.Modified();
        }

        var entity = ShortenedUrl.Hydrate(
            existingEntity.Id,
            LongUrl.Create(command.LongUrl),
            Summary.Create(command.Summary),
            existingEntity.ShortUrlCode,
            existingEntity.UrlMetadata);

        return entity;
    }
}