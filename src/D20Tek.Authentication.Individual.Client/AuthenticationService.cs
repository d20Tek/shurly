//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Client;

internal sealed class AuthenticationService : ServiceBase, IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly JwtAuthenticationProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly string _baseUrl;

    public AuthenticationService(
        HttpClient httpClient,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IOptions<ServiceEndpointSettings> endpointOptions,
        ILogger<AuthenticationService> logger)
        : base(logger)
    {
        _httpClient = httpClient;
        _authStateProvider = (JwtAuthenticationProvider)authStateProvider;
        _localStorage = localStorage;
        _baseUrl = endpointOptions.Value.Authentication;
    }

    public async Task<Result<AuthenticationResponse>> ChangePasswordAsync(
        ChangePasswordRequest request)
    {
        var response = await InvokeServiceOperation<AuthenticationResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.ChangePassword}";
            return await _httpClient.PatchAsJsonAsync(serviceUrl, request);
        },
        UpdateAuthToken);

        return response;
    }

    public async Task<Result<AccountResponse>> DeleteAccountAsync()
    {
        var response = await InvokeServiceOperation<AccountResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Delete}";
            return await _httpClient.DeleteAsync(serviceUrl);
        });

        if (response.IsSuccess is true)
        {
            await LogoutAsync();
        }
        return response;
    }


    public async Task<Result<AccountResponse>> GetAccountAsync()
    {
        var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Get}";
        var result = await _httpClient.GetFromJsonAsync<AccountResponse>(serviceUrl);
        if (result is null)
        {
            return Errors.AuthenticationService.AccountNotFound;
        }

        return result;
    }

    public async Task<Result<ResetResponse>> GetPasswordResetTokenAsync(GetResetTokenRequest request)
    {
        var response = await InvokeServiceOperation<ResetResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.ResetPassword}";
            return await _httpClient.PostAsJsonAsync(serviceUrl, request);
        });

        return response;
    }

    public async Task<Result<AuthenticationResponse>> LoginAsync(LoginRequest request)
    {
        var response = await InvokeServiceOperation<AuthenticationResponse>(async() =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Login}";
            return await _httpClient.PostAsJsonAsync(serviceUrl, request);
        },
        UpdateAuthToken);

        return response;
    }

    public async Task LogoutAsync()
    {
        await InvokeOperation(async () =>
        {
            await _localStorage.RemoveItemAsync(Configuration.Authentication.AccessTokenKey);
            await _localStorage.RemoveItemAsync(Configuration.Authentication.RefreshTokenKey);

            _authStateProvider.NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        });
    }

    public async Task<Result<AuthenticationResponse>> RegisterAsync(RegisterRequest request)
    {
        var response = await InvokeServiceOperation<AuthenticationResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Register}";
            return await _httpClient.PostAsJsonAsync(serviceUrl, request);
        },
        UpdateAuthToken);

        return response;
    }

    public async Task<Result<AuthenticationResponse>> ResetPasswordAsync(
        ResetPasswordRequest request)
    {
        var response = await InvokeServiceOperation<AuthenticationResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.ResetPassword}";
            return await _httpClient.PatchAsJsonAsync(serviceUrl, request);
        },
        UpdateAuthToken);

        return response;
    }

    public async Task<Result<AccountResponse>> UpdateAccountAsync(UpdateProfileRequest request)
    {
        var response = await InvokeServiceOperation<AccountResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Update}";
            return await _httpClient.PutAsJsonAsync(serviceUrl, request);
        });

        return response;
    }

    private async Task UpdateAuthToken(Result<AuthenticationResponse> response)
    {
        if (response.IsSuccess)
        {
            await _localStorage.SetItemAsync(
                Configuration.Authentication.AccessTokenKey,
                response.Value.Token);

            await _localStorage.SetItemAsync(
                Configuration.Authentication.RefreshTokenKey,
                response.Value.RefreshToken);

            _authStateProvider.NotifyUserAuthentication(response.Value.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                Configuration.Authentication.JwtBearerScheme,
                response.Value.Token);
        }
    }
}
