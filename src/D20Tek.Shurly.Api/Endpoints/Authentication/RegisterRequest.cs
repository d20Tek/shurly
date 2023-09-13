//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Shurly.Api.Endpoints.Authentication;

internal sealed record RegisterRequest(
    string UserName,
    string GivenName,
    string FamilyName,
    string Email,
    string Password) : IRequest<IResult>;
