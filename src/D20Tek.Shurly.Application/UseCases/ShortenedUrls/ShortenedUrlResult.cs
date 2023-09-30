//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls;

public sealed record ShortenedUrlResult(
    Guid Id,
    string LongUrl,
    string ShortUrlCode,
    string Summary,
    int State,
    DateTime PublishOn);
