//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace D20Tek.Authentication.Individual.Client;

internal static class JwtAuthenticationStateFactory
{
    private const string _jwtAuthenticationType = "jwtAuthType";

    public static AuthenticationState Create(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(
            claims,
            _jwtAuthenticationType,
            JwtRegisteredClaimNames.Name,
            ClaimTypes.Role);

        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }

    public static AuthenticationState CreateAnonymous()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        return new AuthenticationState(principal);
    }
}
