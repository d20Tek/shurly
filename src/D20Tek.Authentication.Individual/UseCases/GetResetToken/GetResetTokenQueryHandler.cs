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

internal sealed class GetResetTokenQueryHandler : IGetResetTokenQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly GetResetTokenQueryValidator _validator;
    private readonly ILogger _logger;

    public GetResetTokenQueryHandler(
        IUserAccountRepository accountRepository,
        GetResetTokenQueryValidator validator,
        ILogger<GetResetTokenQueryHandler> logger)
    {
        _accountRepository = accountRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<ResetTokenResult>> HandleAsync(
        GetResetTokenQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<ResetTokenResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var existingAccount = await ValidateGuardConditions(query);
                if (existingAccount.IsFailure)
                {
                    return existingAccount.ErrorsList;
                }

                // 2. generate a reset code
                var account = existingAccount.Value;
                var rawCode = await _accountRepository.GeneratePasswordResetTokenAsync(account);
                if (rawCode is null)
                {
                    return Errors.UserAccount.CannotGenerateResetToken;
                }

                // 3. encode the reset token, so it can be used in a url
                var resetCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawCode));
                return new ResetTokenResult(resetCode);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(GetResetTokenQuery query)
    {
        // 1. validate command input
        var vResult = _validator.Validate(query);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. get the existing account record by user's email
        var existingAccount = await _accountRepository.GetByEmailAsync(query.Email);
        if (existingAccount is null)
        {
            return Errors.UserAccount.EmailNotFound(query.Email);
        }

        return existingAccount;
    }
}
