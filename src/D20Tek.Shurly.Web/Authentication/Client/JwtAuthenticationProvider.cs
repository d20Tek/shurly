//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace D20Tek.Authentication.Individual.Client;

internal sealed class JwtAuthenticationProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public JwtAuthenticationProvider(
        HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(
            Configuration.Authentication.AccessTokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return JwtAuthenticationStateFactory.CreateAnonymous();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            Configuration.Authentication.JwtBearerScheme,
            token);

        var tokenClaims = JwtParser.ParseClaimsFromJwt(token);
        return JwtAuthenticationStateFactory.Create(tokenClaims);
    }

    public void NotifyUserAuthentication(string token)
    {
        var claims = JwtParser.ParseClaimsFromJwt(token);
        var authState = JwtAuthenticationStateFactory.Create(claims);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public void NotifyUserLogout()
    {
        var authState = JwtAuthenticationStateFactory.CreateAnonymous();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }
}
