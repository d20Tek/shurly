//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Web.Helpers;

internal class TagsParser
{
    public static List<string> ParseTagEntries(string tagsRaw)
    {
        var tagEntries = tagsRaw.Split(
            ';',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new List<string>(tagEntries);
    }
}
