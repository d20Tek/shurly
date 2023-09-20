//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.Client;

public interface IAuthenticationService
{
    public Task<Result<AccountResponse>> GetAccountAsync();

    public Task<Result<AuthenticationResponse>> LoginAsync(LoginRequest request);

    public Task LogoutAsync();

    public Task<Result<AuthenticationResponse>> RegisterAsync(RegisterRequest request);

    public Task<Result<AccountResponse>> UpdateAccountAsync(UpdateProfileRequest request);
}
