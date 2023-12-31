﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Web.Helpers;

internal class DateTimeHelper
{
    public static DateTime UtcToLocalDateTime(DateTime utcDateTime)
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
        return localDateTime;
    }

    public static DateTime? UtcToLocalDateTime(DateTime? utcDateTime)
    {
        if (utcDateTime is null) return null;
        return UtcToLocalDateTime(utcDateTime.Value);
    }

    public static string UtcToLocalDateTimeString(DateTime utcDateTime, bool useShortFormat)
    {
        var localDateTime = DateTimeHelper.UtcToLocalDateTime(utcDateTime);
        if (useShortFormat is true)
        {
            return localDateTime.ToString("MMM d, yyyy");
        }
        else
        {
            return localDateTime.ToString("MMMM d, yyyy h:mm tt");
        }
    }

    public static DateTime? LocalDateTimeToUtc(DateTime? localDateTime, bool nullChecker)
    {
        if (nullChecker || localDateTime.HasValue is false)
        {
            return null;
        }
        else
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime.Value, localTimeZone);
            return utcDateTime;
        }
    }
}
