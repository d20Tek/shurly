//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Shurly.Api.Endpoints.Authentication;

namespace D20Tek.Authentication.Individual.UseCases.Register;

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
