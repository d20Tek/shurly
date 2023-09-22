//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.Pages.Manage;

public partial class ForgotPassword
{
    private readonly GetResetTokenRequest input = new();
    private string message = string.Empty;
    private string resetCode = string.Empty;

    private bool hasResetCode => !string.IsNullOrEmpty(resetCode);

    private async Task OnPostAsync()
    {
        message = string.Empty;

        var response = await _authService.GetPasswordResetTokenAsync(input);
        response.MatchFirstError(
            success => resetCode = success.ResetToken,
            error => message = error.ToString());
    }
}
