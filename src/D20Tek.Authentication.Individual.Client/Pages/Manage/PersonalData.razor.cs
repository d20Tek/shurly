//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using System.Text.Json;

namespace D20Tek.Authentication.Individual.Client.Pages.Manage;

public partial class PersonalData
{
    private string displayData = string.Empty;

    private bool hasDisplayData => !string.IsNullOrEmpty(displayData);

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private async Task DownloadDataAsync()
    {
        var result = await _authService.GetAccountAsync();
        result.MatchFirstError(
            account => displayData = ProcessAccountData(account),
            error => displayData = $"[{error.Code}]: {error.Message}");
    }

    private string ProcessAccountData(AccountResponse account)
    {
        return JsonSerializer.Serialize<AccountResponse>(account, jsonOptions);
    }

    private async Task DeleteAccountAsync()
    {
        var result = await _authService.DeleteAccountAsync();
        result.MatchFirstError(
            account => _nav.NavigateTo("/"),
            error => displayData = error.ToString());
    }
}
