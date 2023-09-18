//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client;

internal class JwtClientSettings
{
    public string Secret { get; init; } = default!;

    public string Issuer { get; init; } = default!;

    public string Audience { get; init; } = default!;
}
