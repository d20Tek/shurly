//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.RemoveAccount;

internal class RemoveCommandHandler : IRemoveCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly ILogger _logger;

    public RemoveCommandHandler(
        IUserAccountRepository accountRepository,
        ILogger<RemoveCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<Result<AccountResult>> HandleAsync(
        RemoveCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AccountResult>(
            _logger,
            async () =>
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
                    return result.ToMinimalResult<AccountResult>();
                }

                return new AccountResult(command.UserId);
            });
    }
}
