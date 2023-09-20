//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.Register;

public sealed record RegisterCommand(
    string UserName,
    string GivenName,
    string FamilyName,
    string Email,
    string Password,
    string? PhoneNumber) : ICommand<Result<AuthenticationResult>>;

