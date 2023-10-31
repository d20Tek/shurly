//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Services.Contracts;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlList
{
    private IEnumerable<ShortenedUrlResponse>? _urlList;
    private string _message = "Loading...";

    protected override async Task OnInitializedAsync()
    {
        var result = await _service.GetByOwnerAsync();
        result.MatchFirstError(
            success => {
                _message = string.Empty;
                _urlList = success;
            },
            error => _message = error.ToString());
    }
}
