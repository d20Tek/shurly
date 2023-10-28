//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class PublishStateButton
{
    private UrlState LinkState => Link?.State ?? UrlState.New;

    [Parameter]
    public ShortenedUrlResponse? Link { get; set; }

    [Parameter]
    public EventCallback<ShortenedUrlResponse> PublishStateChanged { get; set; }

    private async Task OnPublish()
    {
        if (Link is null) return;

        var result = await _service.PublishAsync(Link.Id);
        result.MatchFirstError(
            success => PublishStateChanged.InvokeAsync(success),
            error => Console.WriteLine($"Publish Error: {error}"));
    }

    private async Task OnUnpublish()
    {
        if (Link is null) return;

        var result = await _service.UnpublishAsync(Link.Id);
        result.MatchFirstError(
            success => PublishStateChanged.InvokeAsync(success),
            error => Console.WriteLine($"Unpublish Error: {error}"));
    }
}
