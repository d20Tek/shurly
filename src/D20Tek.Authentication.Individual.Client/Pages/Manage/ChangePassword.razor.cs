//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.Pages.Manage;

public partial class ChangePassword
{
    private readonly ChangePasswordRequest input = new();
    private string message = string.Empty;

    private async Task OnPostAsync()
    {
        message = string.Empty;

        var response = await _authService.ChangePasswordAsync(input);
        response.MatchFirstError(
            auth =>
            {
                message = "Password changed successfully!";
                _nav.NavigateTo("/authentication/profile");
            },
            error => message = error.ToString());
    }
}
