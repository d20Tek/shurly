//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.RefreshToken;

internal class RefreshTokenQueryHandler : IRefreshTokenQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenQueryHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        RefreshTokenQuery query,
        CancellationToken cancellationToken)
    {
        // 1. ensure account is still valid and available.
        if (await _accountRepository.GetByIdAsync(query.UserId) is not UserAccount account)
        {
            return Errors.UserAccount.NotFound(query.UserId);
        }

        // future: look for revoked tokens before using refresh token to generate new ones.

        // 2. produce a new token
        return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
    }
}
