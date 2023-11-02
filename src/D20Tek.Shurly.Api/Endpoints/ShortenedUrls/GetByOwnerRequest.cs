//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record GetByOwnerRequest(
    HttpContext Context,
    ClaimsPrincipal User,
    int Skip = 0,
    int Take = 25) : IRequest<IResult>;
