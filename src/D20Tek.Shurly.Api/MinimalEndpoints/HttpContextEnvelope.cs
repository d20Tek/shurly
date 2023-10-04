//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace D20Tek.Minimal.Endpoints;

public sealed record HttpContextEnvelope<TBody>(
    HttpContext Context,
    ClaimsPrincipal User,
    TBody Body) : IRequest<IResult>
        where TBody : class;

public sealed record HttpContextRequest(
    HttpContext Context,
    ClaimsPrincipal User) : IRequest<IResult>;
