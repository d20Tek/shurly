//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record ShortenedUrlListResponse(
    ListMetadata Metadata,
    List<LinkMetadata> Links,
    List<ShortenedUrlResponse> Items);

internal sealed record ListMetadata(int TotalItems, int ItemsCount, int Skip, int Take);

internal sealed record LinkMetadata(string Type, string Url);
