//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.Client;

internal interface IAuthenticationService
{
    public Task<Result<AuthenticationResponse>> LoginAsync(LoginRequest request);

    public Task LogoutAsync();
}
