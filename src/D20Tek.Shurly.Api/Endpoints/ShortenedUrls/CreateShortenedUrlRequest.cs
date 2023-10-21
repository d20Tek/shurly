﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record CreateShortenedUrlRequest(
    string Title,
    string LongUrl,
    string? Summary = null,
    List<string>? Tags = null,
    DateTime? PublishOn = null) : IRequest<IResult>;
