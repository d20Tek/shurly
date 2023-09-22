//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.Pages.Manage;

public partial class ResetPassword
{
    private readonly ResetPasswordRequest input = new();
    private string message = string.Empty;

    [Parameter]
    public string ResetCode { get; set; } = default!;

    private async Task OnPostAsync()
    {
        message = string.Empty;

        input.ResetToken = ResetCode;
        var response = await _authService.ResetPasswordAsync(input);
        response.MatchFirstError(
            auth =>
            {
                message = "Password reset successfully!";
                _nav.NavigateTo("/");
            },
            error => message = error.ToString());
    }
}
