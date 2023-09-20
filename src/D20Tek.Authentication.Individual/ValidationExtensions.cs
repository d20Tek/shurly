//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace D20Tek.Authentication.Individual;

internal static class ValidationExtensions
{
    private const string _intlNumberRegex = "^\\+[0-9][0-9\\s-]*$";
    private const string _phoneNumberRegex = "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

    public static bool IsValidPhoneNumber(this string text)
    {
        var result = Regex.Match(text, _phoneNumberRegex);
        if (result.Success is false)
        {
            result = Regex.Match(text, _intlNumberRegex);
        }
        return result.Success;
    }
}
