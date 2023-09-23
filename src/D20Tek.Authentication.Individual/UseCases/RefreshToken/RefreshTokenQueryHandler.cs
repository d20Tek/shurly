//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.RefreshToken;

internal class RefreshTokenQueryHandler : IRefreshTokenQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger _logger;

    public RefreshTokenQueryHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<RefreshTokenQueryHandler> logger)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        RefreshTokenQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AuthenticationResult>(
            _logger,
            async () =>
            {
                // 1. ensure account is still valid and available.
                if (await _accountRepository.GetByIdAsync(query.UserId) is not UserAccount account)
                {
                    return Errors.UserAccount.NotFound(query.UserId);
                }

                // future: look for revoked tokens before using refresh token to generate new ones.

                // 2. produce a new token
                return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
            });
    }
}
