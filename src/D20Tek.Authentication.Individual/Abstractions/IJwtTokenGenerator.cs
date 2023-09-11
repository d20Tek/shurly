//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.IdentityModel.Tokens;

namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IJwtTokenGenerator
{
    public SigningCredentials GetSigningCredentials();

    public string GenerateAccessToken(UserAccount account, IEnumerable<string> userRoles);

    public string GenerateRefreshToken(UserAccount account);
}
