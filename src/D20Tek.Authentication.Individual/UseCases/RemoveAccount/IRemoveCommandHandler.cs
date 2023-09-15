//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.RemoveAccount;

public interface IRemoveCommandHandler :
    ICommandHandler<RemoveCommand, Result<AccountResult>>
{
}
