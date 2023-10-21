//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;

public sealed record UpdateShortenedUrlCommand(
    Guid ShortUrlId,
    string Title,
    string LongUrl,
    string? Summary,
    Guid OwnerId,
    List<string>? Tags = null,
    DateTime? PublishOn = null) : ICommand<Result<ShortenedUrlResult>>;
