//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Authentication.Individual.Api;

public sealed record ResetPasswordRequest(
    string Email,
    string ResetToken,
    string NewPassword) : IRequest<IResult>;
