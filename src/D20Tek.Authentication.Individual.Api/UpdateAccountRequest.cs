//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Authentication.Individual.Api;

internal sealed record UpdateAccountRequest(
    string UserName,
    string GivenName,
    string FamilyName,
    string Email,
    string? PhoneNumber) : IRequest<IResult>;
