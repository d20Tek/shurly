//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;

internal class CreateShortenedUrlCommandHandler : ICreateShortenedUrlCommandHandler
{
    private readonly IShortenedUrlRepository _repository;
    private readonly IUrlShorteningService _urlService;
    private readonly CreateShortenedUrlCommandValidator _validator;
    private readonly ILogger _logger;

    public CreateShortenedUrlCommandHandler(
        IShortenedUrlRepository repository,
        IUrlShorteningService urlService,
        CreateShortenedUrlCommandValidator validator,
        ILogger<CreateShortenedUrlCommandHandler> logger)
    {
        _repository = repository;
        _urlService = urlService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<ShortenedUrlResult>> HandleAsync(
        CreateShortenedUrlCommand command,
        CancellationToken cancellation)
    {
        return await UseCaseOperation.InvokeAsync<ShortenedUrlResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var guardResult = ValidateGuardConditions(command);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. create shortened url entity
                var entity = await CreateEntity(command);

                // 3. persist the new entity
                var created = await _repository.CreateAync(entity);
                if (created is false)
                {
                    return DomainErrors.CreateFailed;
                }

                return ShortenedUrlResult.FromEntity(entity);
            });
    }

    private Result ValidateGuardConditions(CreateShortenedUrlCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<ShortenedUrlResult>();
        }

        // 2. check if url string is valid
        if (!Uri.TryCreate(command.LongUrl, UriKind.Absolute, out _))
        {
            return DomainErrors.LongUrlInvalidFormat;
        }

        return Result.Success();
    }

    private async Task<ShortenedUrl> CreateEntity(CreateShortenedUrlCommand command)
    {
        var code = await _urlService.GenerateUniqueCodeAsync();
        var entity = ShortenedUrl.Create(
            LongUrl.Create(command.LongUrl),
            Summary.Create(command.Summary),
            ShortUrlCode.Create(code),
            AccountId.Create(command.CreatorId),
            command.PublishOn);

        return entity;
    }
}
