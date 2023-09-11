//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Infrastructure;

internal class JwtSettings
{
    public string Secret { get; init; } = default!;

    public int ExpiryMinutes { get; init; } = default!;

    public string Issuer { get; init; } = default!;

    public string Audience { get; init; } = default!;

    public string[] Scopes { get; init; } = default!;

    public int RefreshExpiryDays { get; init; } = default!;

    public string[] RefreshScopes { get; init; } = default!;
}
