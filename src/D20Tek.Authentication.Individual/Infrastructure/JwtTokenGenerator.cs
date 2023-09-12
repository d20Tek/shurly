//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeFacade _dateTimeFacade;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IDateTimeFacade dateTimeFacade, IOptions<JwtSettings> jwtOptions)
    {
        this._dateTimeFacade = dateTimeFacade;
        _jwtSettings = jwtOptions.Value;
    }

    public SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(
            GetSecurityKey(_jwtSettings.Secret),
            SecurityAlgorithms.HmacSha256);
    }

    internal static SecurityKey GetSecurityKey(string secret) =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

    public TokenResponse GenerateAccessToken(UserAccount account, IEnumerable<string> userRoles)
    {
        List<Claim> claims = GetClaims(account, _jwtSettings.Scopes, userRoles);

        var token = CreateJwtToken(
            claims,
            _dateTimeFacade.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes));

        return token;
    }

    public TokenResponse GenerateRefreshToken(UserAccount account)
    {
        List<Claim> claims = GetClaims(account, _jwtSettings.RefreshScopes, null);

        var token = CreateJwtToken(
            claims,
            _dateTimeFacade.UtcNow.AddDays(_jwtSettings.RefreshExpiryDays));

        return token;
    }

    private List<Claim> GetClaims(
        UserAccount account,
        string[] scopes,
        IEnumerable<string>? userRoles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, account.GivenName),
            new Claim(JwtRegisteredClaimNames.FamilyName, account.FamilyName),
            new Claim(JwtRegisteredClaimNames.Name, account.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, account.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var scope in scopes)
        {
            claims.Add(new Claim(ClaimTypesExtension.Scope, scope));
        }

        if (userRoles is not null)
        {
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        return claims;
    }

    private TokenResponse CreateJwtToken(List<Claim> claims, DateTime expiresOn)
    {
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: expiresOn,
            claims: claims,
            signingCredentials: GetSigningCredentials());

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse(tokenString, token.ValidTo);
    }
}
