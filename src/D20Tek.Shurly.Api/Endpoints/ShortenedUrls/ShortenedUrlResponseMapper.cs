﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed class ShortenedUrlResponseMapper
{
    public ShortenedUrlResponse Map(ShortenedUrlResult source, HttpContext httpContext)
    {
        return new ShortenedUrlResponse(
            source.Id.ToString(),
            source.LongUrl,
            $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{source.ShortUrlCode}",
            source.Summary,
            source.State,
            source.PublishOn);
    }
}