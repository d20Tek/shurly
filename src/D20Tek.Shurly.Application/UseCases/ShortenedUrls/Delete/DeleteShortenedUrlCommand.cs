//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;

public sealed record DeleteShortenedUrlCommand(
    Guid ShortUrlId,
    Guid OwnerId) : ICommand<Result<ShortenedUrlResult>>;
