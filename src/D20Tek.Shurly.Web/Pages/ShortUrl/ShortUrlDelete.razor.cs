//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlDelete
{
    private string _message = "";
    private string _shortUrl = "";
    private string _linkName = "";

    [Parameter]
    public string Id { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var result = await _service.GetByIdAsync(Id);
        result.MatchFirstError(
            success =>
            {
                _shortUrl = success.ShortUrl;
                _linkName = success.Title;
            },
            error =>
            {
                _message = error.ToString();
                _shortUrl = "invalid-link";
            });
    }

    private async Task OnCancel() =>
        await _nav.GoBack(_jsRuntime);

    private async Task OnDelete()
    {
        var result = await _service.DeleteAsync(Id);
        if (result.IsSuccess)
        {
            _nav.NavigateTo(Configuration.ShortUrl.ListUrl);
        }
        else
        {
            _message = result.Errors.First().ToString();
        }
    }
}
