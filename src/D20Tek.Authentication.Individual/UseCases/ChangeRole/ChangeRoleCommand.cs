//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ChangeRole;

public sealed record ChangeRoleCommand(
    string UserName,
    string NewRole) : ICommand<Result>;
