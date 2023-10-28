//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Web;

internal static class Configuration
{
    public static class ShortUrl
    {
        public const string ListUrl = $"/short-url";

        public const string CreateUrl = $"/short-url/create";

        public static string EditUrl(string id) => $"/short-url/edit/{id}";

        public static string DeleteUrl(string id) => $"/short-url/delete/{id}";

        public static string ViewUrl(string id) => $"/short-url/view/{id}";
    }
}
