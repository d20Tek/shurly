//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases;
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Authentication.Individual.Api;

internal sealed class AuthenticationResponseMapper :
    IMapper<AuthenticationResult, AuthenticationResponse>
{
    public AuthenticationResponse Map(AuthenticationResult source)
    {
        return new AuthenticationResponse(
            source.UserId,
            source.UserName,
            source.Token,
            source.Expiration,
            source.RefreshToken);
    }
}
