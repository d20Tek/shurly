//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace D20Tek.Authentication.Individual.Client;

internal sealed class JwtAuthenticationProvider : AuthenticationStateProvider
{
    private const string _jwtAuthenticationType = "jwtAuthType";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous = new (new ClaimsPrincipal());
    private readonly JwtClientSettings _jwtSettings;

    public JwtAuthenticationProvider(
        HttpClient httpClient,
        IOptions<JwtClientSettings> jwtOptions,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _jwtSettings = jwtOptions.Value;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(
            Configuration.Authentication.AccessTokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return _anonymous;
        }

        var principal = DecodeJwtToken(token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            Configuration.Authentication.JwtBearerScheme,
            token);

        return new AuthenticationState(principal);
    }

    public void NotifyUserAuthentication(string token)
    {
        var principal = DecodeJwtToken(token);
        var authState = new AuthenticationState(principal);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private ClaimsPrincipal DecodeJwtToken(string token)
    {
        try
        {
            // define the token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                NameClaimType = JwtRegisteredClaimNames.Name,

                AuthenticationType = _jwtAuthenticationType,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // validate and decode the token
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken validatedToken);

            return claimsPrincipal;
        }
        catch (Exception ex)
        {
            // nandle exceptions here and return anonymous principal
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return new ClaimsPrincipal();
        }
    }
}
