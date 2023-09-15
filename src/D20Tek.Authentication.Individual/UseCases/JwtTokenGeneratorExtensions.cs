//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;

namespace D20Tek.Authentication.Individual.UseCases;

internal static class JwtTokenGeneratorExtensions
{
    public async static Task<AuthenticationResult> GenerateTokenResult(
        this IJwtTokenGenerator jwtTokenGenerator,
        IUserAccountRepository repository,
        UserAccount userAccount)
    {
        var userRoles = await repository.GetUserRolesAsync(userAccount);
        var token = jwtTokenGenerator.GenerateAccessToken(userAccount, userRoles);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken(userAccount);

        return new AuthenticationResult(
            userAccount.Id,
            userAccount.UserName!,
            token.Token,
            token.ValidTo,
            refreshToken.Token);
    }
}
