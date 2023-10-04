﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record ShortenedUrlResponse(
    string Id,
    string LongUrl,
    string ShortUrl,
    string Summary,
    int State,
    DateTime PublishOn);