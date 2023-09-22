//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.Pages.Manage;

public partial class Profile
{
    private UpdateProfileRequest input = new();
    private string message = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var result = await _authService.GetAccountAsync();
        result.MatchFirstError(
            account => input = new UpdateProfileRequest
            {
                UserName = account.UserName,
                GivenName = account.GivenName,
                FamilyName = account.FamilyName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber
            },
            error => message = $"[{error.Code}]: {error.Message}");
    }

    private async Task OnPostAsync()
    {
        message = string.Empty;

        var response = await _authService.UpdateAccountAsync(input);
        response.MatchFirstError(
            success => message = "Account profile saved.",
            error => message = error.ToString());
    }
}
