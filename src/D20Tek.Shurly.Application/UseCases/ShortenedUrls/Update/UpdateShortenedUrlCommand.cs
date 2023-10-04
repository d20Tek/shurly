//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;

public sealed record UpdateShortenedUrlCommand(
    Guid ShortUrlId,
    string LongUrl,
    string Summary,
    Guid CreatorId,
    DateTime? PublishOn = null) : ICommand<Result<ShortenedUrlResult>>;
