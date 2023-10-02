//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed record GetByShortCodeRequest(string shortCode) : IRequest<IResult>;
