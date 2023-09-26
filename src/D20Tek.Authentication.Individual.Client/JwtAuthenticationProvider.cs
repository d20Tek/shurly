//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;

    public JwtAuthenticationProvider(
        HttpClient httpClient,
        IOptions<JwtClientSettings> jwtOptions,
        ILocalStorageService localStorage,
        ILogger<JwtAuthenticationProvider> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogInformation($"==> GetAuthenticationStateAsync started.");
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

        _logger.LogInformation($"==> GetAuthenticationStateAsync completed.");
        return new AuthenticationState(principal);
    }

    public void NotifyUserAuthentication(string token)
    {
        _logger.LogInformation($"==> NotifyUserAuthentication called.");
        var principal = DecodeJwtToken(token);
        var authState = new AuthenticationState(principal);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public void NotifyUserLogout()
    {
        _logger.LogInformation($"==> NotifyUserLogout called.");
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private ClaimsPrincipal DecodeJwtToken(string token)
    {
        try
        {
            _logger.LogInformation($"==> DecodeJwtToken started.");

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

            _logger.LogInformation($"Token validation succeeded and ClaimsPrincipal returned.");
            return claimsPrincipal;
        }
        catch (Exception ex)
        {
            // handle exceptions here and return anonymous principal
            _logger.LogError($"Token validation failed: {ex.Message}", ex);
            return new ClaimsPrincipal();
        }
    }
}
