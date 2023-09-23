//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.ChangePassword;

internal class ChangePasswordCommandHandler : IChangePasswordCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ChangePasswordCommandValidator _validator;
    private readonly ILogger _logger;

    public ChangePasswordCommandHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ChangePasswordCommandValidator validator,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AuthenticationResult>(
            _logger,
            async() =>
            {
                // 1. test guard conditions
                var guardResult = await ValidateGuardConditions(command);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. check if current password is correct
                var account = guardResult.Value;
                if (!await _accountRepository.CheckPasswordAsync(account, command.CurrentPassword))
                {
                    return Errors.Authentication.InvalidExistingCredentials;
                }

                // 3. perform the update
                var result = await _accountRepository.ChangePasswordAsync(
                    account,
                    command.CurrentPassword,
                    command.NewPassword);
                if (result.Succeeded is false)
                {
                    return Errors.UserAccount.ChangePasswordForbidden;
                }

                return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(ChangePasswordCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. validate user exists
        if (await _accountRepository.GetByIdAsync(command.UserId) is not UserAccount account)
        {
            return Errors.UserAccount.NotFound(command.UserId);
        }

        return account;
    }
}
