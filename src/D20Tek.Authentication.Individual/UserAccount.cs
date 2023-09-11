//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;

namespace D20Tek.Authentication.Individual;

internal class UserAccount : IdentityUser
{
    public string GivenName { get; set; } = string.Empty;

    public string FamilyName { get; set; } = string.Empty;
}
