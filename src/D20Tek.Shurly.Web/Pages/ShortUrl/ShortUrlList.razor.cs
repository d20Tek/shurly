//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Services.Contracts;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlList
{
    private List<ShortenedUrlResponse> _urlList = new();
    private string _message = "Loading...";
    private string? _nextLink;
    private bool _hasMoreResults = false;

    protected override async Task OnInitializedAsync() => await ProcessListRetrieval();

    private async Task OnMoreLinks() => await ProcessListRetrieval(_nextLink);

    private async Task ProcessListRetrieval(string? getUrl = null)
    {
        var result = await _service.GetByOwnerAsync(getUrl);
        result.MatchFirstError(
            success => {
                _message = string.Empty;
                _urlList.AddRange(success.Items);
                _nextLink = success.Links.FirstOrDefault(x => x.Type == "nextLink")?.Url;
                _hasMoreResults = !string.IsNullOrEmpty(_nextLink);
            },
            error => _message = error.ToString());
    }
}
