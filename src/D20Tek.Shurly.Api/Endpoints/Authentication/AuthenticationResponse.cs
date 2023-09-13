//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Api.Endpoints.Authentication;

internal sealed record AuthenticationResponse(
    string UserId,
    string UserName,
    string Token,
    DateTime Expiration,
    string RefreshToken);
