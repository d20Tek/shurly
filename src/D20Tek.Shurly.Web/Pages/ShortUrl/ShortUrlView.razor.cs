//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Helpers;
using D20Tek.Shurly.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlView
{
    private ShortenedUrlResponse? _link;
    private string _message = "";

    private bool _hasFuturePublishOn => _link!.PublishOn > DateTime.UtcNow;

    private DateTime _localPublishOn => DateTimeHelper.UtcToLocalDateTime(_link!.PublishOn);

    [Parameter]
    public string Id { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var result = await _service.GetByIdAsync(Id);
        result.MatchFirstError(
            success => _link = success,
            error => _message = error.ToString());
    }

    private void OnPublishStateChanged(ShortenedUrlResponse newResponse)
    {
        _link = newResponse;
    }
}
