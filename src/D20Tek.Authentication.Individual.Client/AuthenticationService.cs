//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Components.Authorization;
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
        IOptions<ServiceEndpointSettings> endpointOptions)
    {
        _httpClient = httpClient;
        _authStateProvider = (JwtAuthenticationProvider)authStateProvider;
        _localStorage = localStorage;
        _baseUrl = endpointOptions.Value.Authentication;
    }

    public async Task<Result<AuthenticationResponse>> LoginAsync(LoginRequest request)
    {
        var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Login}";
        var loginResult = await _httpClient.PostAsJsonAsync(serviceUrl, request);
        var response = await ProcessHttpResponse<AuthenticationResponse>(loginResult);

        if (response.IsSuccess)
        {
            await UpdateAuthToken(response.Value);
        }

        return response;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(Configuration.Authentication.AccessTokenKey);
        await _localStorage.RemoveItemAsync(Configuration.Authentication.RefreshTokenKey);

        _authStateProvider.NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<Result<AuthenticationResponse>> RegisterAsync(RegisterRequest request)
    {
        var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Register}";
        var registerResult = await _httpClient.PostAsJsonAsync(serviceUrl, request);
        var response = await ProcessHttpResponse<AuthenticationResponse>(registerResult);

        if (response.IsSuccess)
        {
            await UpdateAuthToken(response.Value);
        }

        return response;
    }

    private async Task UpdateAuthToken(AuthenticationResponse response)
    {
        await _localStorage.SetItemAsync(
            Configuration.Authentication.AccessTokenKey,
            response.Token);

        await _localStorage.SetItemAsync(
            Configuration.Authentication.RefreshTokenKey,
            response.RefreshToken);

        _authStateProvider.NotifyUserAuthentication(response.Token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            Configuration.Authentication.JwtBearerScheme,
            response.Token);
    }
}
