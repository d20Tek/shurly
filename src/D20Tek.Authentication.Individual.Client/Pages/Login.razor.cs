//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.Pages;

public partial class Login
{
    private readonly LoginRequest request = new();
    private string message = string.Empty;

    private async Task OnPostAsync()
    {
        message = "Wait...";

        var response = await _authService.LoginAsync(request);
        response.MatchFirstError(
            success => _nav.NavigateTo("/"),
            error => message = error.ToString());
    }
}
