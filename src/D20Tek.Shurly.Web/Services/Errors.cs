//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Web.Services;

internal static class Errors
{
    public static class ShortenedUrlService
    {
        public static readonly Error GetShortUrlFailure = Error.Failure(
            "ShortenedUrl.Failure",
            "Unable to perform service operation to retrieve short urls.");

        public static readonly Error CannotParseProblem = Error.Invalid(
            "Service.CannotParseProblem",
            "Unable to parse a problem details from the service response body");

        public static readonly Error CannotConvertPayload = Error.Invalid(
            "Service.CannotConvertPayload",
            "Invalid format of the expected message response.");
    }
}
