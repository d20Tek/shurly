//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Api;

internal sealed record AuthenticationResponse(
    string UserId,
    string UserName,
    string Token,
    DateTime Expiration,
    string RefreshToken);
