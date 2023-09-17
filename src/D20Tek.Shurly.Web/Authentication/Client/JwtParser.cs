//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;
using System.Text.Json;

namespace D20Tek.Authentication.Individual.Client;

internal static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var claims = new List<Claim>();
        var payload = token.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);
        
        var kvPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (kvPairs is not null)
        {
            var parsedClaims = kvPairs.Select(
                kv => new Claim(kv.Key, kv.Value.ToString()!));

            claims.AddRange(parsedClaims);
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64Text)
    {
        switch(base64Text.Length %4)
        {
            case 2: base64Text += "=="; break;
            case 3: base64Text += "="; break;
        }

        return Convert.FromBase64String(base64Text);
    }
}
