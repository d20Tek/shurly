//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace D20Tek.Authentication.Individual.Client.Contracts;

internal sealed class LoginRequest
{
    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
