//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Api;

internal sealed record AccountResponse(
    string UserId,
    string? UserName,
    string? GivenName,
    string? FamilyName,
    string? Email,
    string? PhoneNumber);