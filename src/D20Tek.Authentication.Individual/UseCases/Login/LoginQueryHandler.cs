//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.Login;

internal sealed class LoginQueryHandler : ILoginQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginQueryValidator _validator;
    private readonly ILogger _logger;

    public LoginQueryHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        LoginQueryValidator validator,
        ILogger<LoginQueryHandler> logger)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AuthenticationResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var guardResult = await ValidateGuardConditions(query);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. validate password matches the one in the identity store
                var account = guardResult.Value;

                if (await _accountRepository.CheckPasswordAsync(account, query.Password) is false)
                {
                    return Errors.Authentication.InvalidCredentials;
                }

                return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(LoginQuery query)
    {
        // 1. validate command input
        var vResult = _validator.Validate(query);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. validate user exists
        if (await _accountRepository.GetByUserNameAsync(query.UserName) is not UserAccount account)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        return account;
    }
}
