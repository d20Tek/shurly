//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls;

public sealed record ShortenedUrlList(
    int Skip,
    int Take,
    int TotalItems,
    IEnumerable<ShortenedUrlResult> Items);
