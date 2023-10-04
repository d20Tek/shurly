//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using System.Security.Claims;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record UpdateShortenedUrlRequest(
    Guid Id,
    UpdateShortenedUrlRequest.RequestBody Body,
    HttpContext Context,
    ClaimsPrincipal User) : IRequest<IResult>
{
    internal sealed record RequestBody(
        string LongUrl,
        string Summary,
        DateTime? PublishOn = null);
}
