//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Authentication.Individual.Api;

internal sealed record RegisterRequest(
    string UserName,
    string GivenName,
    string FamilyName,
    string Email,
    string Password,
    string? PhoneNumber) : IRequest<IResult>;
