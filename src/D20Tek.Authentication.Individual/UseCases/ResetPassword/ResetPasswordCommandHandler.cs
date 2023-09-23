//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace D20Tek.Authentication.Individual.UseCases.ResetPassword;

internal class ResetPasswordCommandHandler : IResetPasswordCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ResetPasswordCommandValidator _validator;
    private readonly ILogger _logger;

    public ResetPasswordCommandHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ResetPasswordCommandValidator validator,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _validator = validator;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AuthenticationResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var existingAccount = await ValidateGuardConditions(command);
                if (existingAccount.IsFailure)
                {
                    return existingAccount.ErrorsList;
                }

                // 2. decode the reset token
                var token = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(command.ResetToken));

                // 3. reset the password with repository
                var account = existingAccount.Value;
                var result = await _accountRepository.ResetPasswordAsync(
                    account,
                    token,
                    command.NewPassword);
                if (result.Succeeded is false)
                {
                    return Errors.UserAccount.ChangePasswordForbidden;
                }

                return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(ResetPasswordCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. get the existing account record by user's email
        var existingAccount = await _accountRepository.GetByEmailAsync(command.Email);
        if (existingAccount is null)
        {
            return Errors.UserAccount.EmailNotFound(command.Email);
        }

        return existingAccount;
    }
}
