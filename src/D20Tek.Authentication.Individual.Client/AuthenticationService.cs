﻿//---------------------------------------------------------------------------------------------------------------------
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
