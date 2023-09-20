//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.UseCases;

public sealed record AccountResult(
    Guid UserId,
    string? UserName = null,
    string? GivenName = null,
    string? FamilyName = null,
    string? Email = null,
    string? PhoneNumber = null);