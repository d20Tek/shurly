//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.UseCases;

public sealed record AuthenticationResult(
    string UserId,
    string UserName,
    string Token,
    DateTime Expiration,
    string RefreshToken);
