//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Helpers;
using D20Tek.Shurly.Web.Services.Contracts;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlCreate
{
    private readonly CreateShortenedUrlRequest _request = new();
    private string _message = "";

    private void OnCancel()
    {
        _nav.NavigateTo(Configuration.ShortUrl.ListUrl);
    }

    private async Task OnPostAsync()
    {
        _request.Tags = TagsParser.ParseTagEntries(_request.TagsRaw);

        _request.PublishOn = DateTimeHelper.LocalDateTimeToUtc(
            _request.LocalPublishOn,
            _request.HasPublishDate is false);

        var response = await _service.CreateAsync(_request);
        response.MatchFirstError(
            success => _nav.NavigateTo(Configuration.ShortUrl.ListUrl),
            error => _message = error.ToString());
    }
}
