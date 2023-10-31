//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Shurly.Web.Services;

internal class ClipboardService
{
    private const string _copyTextOperationName = "clipboardCopy.copyText";
    private const string _copyElementOperationName = "clipboardCopy.copyElement";
    private readonly IJSRuntime _jsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task CopyText(string? text)
    {
        if (text is null) return;

        await _jsRuntime.InvokeVoidAsync(_copyTextOperationName, text);
    }

    public async Task CopyElement(ElementReference element)
    {
        await _jsRuntime.InvokeVoidAsync(_copyElementOperationName, element);
    }
}
