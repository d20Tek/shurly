//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.UpdateAccount;

internal class UpdateCommandHandler : IUpdateCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly UpdateCommandValidator _validator;
    private readonly ILogger _logger;

    public UpdateCommandHandler(
        IUserAccountRepository accountRepository,
        UpdateCommandValidator validator,
        ILogger<UpdateCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<AccountResult>> HandleAsync(
        UpdateCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AccountResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var existingAccount = await ValidateGuardConditions(command);
                if (existingAccount.IsFailure)
                {
                    return existingAccount.ErrorsList;
                }

                // 2. Modify the account with command entries.
                var account = existingAccount.Value;
                account.UserName = command.UserName;
                account.GivenName = command.GivenName;
                account.FamilyName = command.FamilyName;
                account.Email = command.Email;
                account.PhoneNumber = command.PhoneNumber;

                // 3. Perform the update
                var result = await _accountRepository.UpdateAsync(account);
                if (result.Succeeded is false)
                {
                    return result.ToMinimalResult<AccountResult>();
                }

                return new AccountResult(
                    new Guid(account.Id),
                    account.UserName,
                    account.GivenName,
                    account.FamilyName,
                    account.Email,
                    account.PhoneNumber);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(UpdateCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. get the existing account record
        var existingAccount = await _accountRepository.GetByIdAsync(command.UserId);
        if (existingAccount is null)
        {
            return Errors.UserAccount.NotFound(command.UserId);
        }

        // 3. verify user isn't trying to use another account's user name
        var nameAccount = await _accountRepository.GetByUserNameAsync(command.UserName);
        if ((nameAccount is not null) && (nameAccount.Id != existingAccount.Id))
        {
            return Errors.UserAccount.UserNameAlreadyInUse;
        }

        // 4. verify user isn't trying to use another account's email
        var emailAccount = await _accountRepository.GetByEmailAsync(command.Email);
        if ((emailAccount is not null) && (emailAccount.Id != existingAccount.Id))
        {
            return Errors.UserAccount.EmailAlreadyInUse;
        }

        return existingAccount;
    }
}
