//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.RemoveAccount;

internal class RemoveCommandHandler : IRemoveCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;

    public RemoveCommandHandler(IUserAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<AccountResult>> HandleAsync(
        RemoveCommand command,
        CancellationToken cancellationToken)
    {
        // 1. get user's account
        if (await _accountRepository.GetByIdAsync(command.UserId) is not UserAccount account)
        {
            return Errors.UserAccount.NotFound(command.UserId);
        }

        // 2. delete that account
        var result = await _accountRepository.DeleteAsync(account);
        if (result.Succeeded is false)
        {
            return Errors.UserAccount.CannotDelete;
        }

        return new AccountResult(command.UserId);
    }
}
