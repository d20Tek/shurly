//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Helpers;
using D20Tek.Shurly.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Shurly.Web.Pages.ShortUrl;

public partial class ShortUrlEdit
{
    private UpdateShortenedUrlRequest _request = new();
    private bool _submitDisabled = false;
    private string _message = "";
    private bool _canceled = false;

    [Parameter]
    public string Id { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var result = await _service.GetByIdAsync(Id);
        result.MatchFirstError(
            success => _request = success.ToUpdateRequest(),
            error =>
            {
                _message = error.ToString();
                _submitDisabled = true;
            });
    }

    private async Task OnCancel()
    {
        await _nav.GoBack(_jsRuntime);
        _canceled = true;
    }

    private async Task OnPostAsync()
    {
        if (_canceled is true) return;

        _request.Tags = TagsParser.ParseTagEntries(_request.TagsRaw);

        _request.PublishOn = DateTimeHelper.LocalDateTimeToUtc(
            _request.LocalPublishOn,
            _request.HasPublishDate is false);

        var response = await _service.UpdateAsync(Id, _request);
        await response.MatchFirstErrorAsync(
            async (success) => await _nav.GoBack(_jsRuntime),
            error =>
            {
                _message = error.ToString();
                return Task.CompletedTask;
            });
    }
}
