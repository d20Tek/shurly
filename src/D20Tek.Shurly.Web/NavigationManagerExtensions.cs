//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace D20Tek.Shurly.Web;

public static class NavigationManagerExtensions
{
    public static async Task GoBack(this NavigationManager nav, IJSRuntime jsRuntime)
    {
        await jsRuntime.InvokeVoidAsync("history.back");
    }

    public static async Task GoForward(this NavigationManager nav, IJSRuntime jsRuntime)
    {
        await jsRuntime.InvokeVoidAsync("history.forward");
    }
}
