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

internal sealed class AuthenticationService : IAuthenticationService
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

    public async Task<Result<AuthenticationResponse>> LoginAsync(LoginRequest model)
    {
        var serviceUrl = $"{_baseUrl}{Configuration.Authentication.Login}";
        var loginResult = await _httpClient.PostAsJsonAsync(serviceUrl, model);
        if (loginResult.IsSuccessStatusCode is false)
        {
            Console.WriteLine("Login error:");
            Console.WriteLine(await loginResult.Content.ReadAsStringAsync());
            return Error.Invalid(
                "Login.Failed",
                "Unable to login with specified user credentials.");
        }

        var response = await loginResult.Content.ReadFromJsonAsync<AuthenticationResponse>();
        if (response is null)
        {
            return Error.Invalid("Login.Failed", "Invalid format of the authentication response.");
        }

        await _localStorage.SetItemAsync(
            Configuration.Authentication.AccessTokenKey,
            response.Token);

        _authStateProvider.NotifyUserAuthentication(response.Token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            Configuration.Authentication.JwtBearerScheme,
            response.Token);
        
        return response;

    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(Configuration.Authentication.AccessTokenKey);

        _authStateProvider.NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
