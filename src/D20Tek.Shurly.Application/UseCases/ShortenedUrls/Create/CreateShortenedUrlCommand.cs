//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;

public sealed record CreateShortenedUrlCommand(
    string Title,
    string LongUrl,
    string? Summary,
    Guid CreatorId,
    List<string>? Tags = null,
    DateTime? PublishOn = null) : ICommand<Result<ShortenedUrlResult>>;
