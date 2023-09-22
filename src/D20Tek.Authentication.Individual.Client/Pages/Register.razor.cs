//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.Pages;

public partial class Register
{
    private readonly RegisterRequest request = new();
    private string message = string.Empty;

    private async Task OnPostAsync()
    {
        var response = await _authService.RegisterAsync(request);
        response.MatchFirstError(
            success => _nav.NavigateTo("/"),
            error => message = error.ToString());
    }
}
