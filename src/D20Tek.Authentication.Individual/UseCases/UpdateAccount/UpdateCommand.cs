//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.UpdateAccount;

public sealed record UpdateCommand(
    Guid UserId,
    string UserName,
    string GivenName,
    string FamilyName,
    string Email,
    string? PhoneNumber) : ICommand<Result<AccountResult>>;
