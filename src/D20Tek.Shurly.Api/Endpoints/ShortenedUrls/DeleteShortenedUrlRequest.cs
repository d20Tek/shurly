//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

public sealed record DeleteShortenedUrlRequest(
    Guid Id,
    HttpContext Context,
    ClaimsPrincipal User) : IRequest<IResult>;
