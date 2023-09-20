//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace D20Tek.Authentication.Individual.Client.Contracts;

public class UpdateProfileRequest
{
    [Required]
    [StringLength(64, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "User name")]
    public string UserName { get; set; } = default!;

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Given name")]
    public string GivenName { get; set; } = default!;

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Family name")]
    public string FamilyName { get; set; } = default!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Phone]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }
}
