//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Authentication.Individual.Api;

public sealed record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword) : IRequest<IResult>;
