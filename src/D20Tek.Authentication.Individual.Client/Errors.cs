//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.Client;

internal static class Errors
{
    public static class AuthenticationService
    {
        public static readonly Error AccountNotFound = Error.NotFound(
            "Account.NotFound",
            "Your user account was not found.");

        public static readonly Error CannotParseProblem = Error.Invalid(
            "Service.CannotParseProblem",
            "Unable to parse a problem details from the service response body");

        public static readonly Error CannotConvertPayload = Error.Invalid(
            "Service.CannotConvertPayload",
            "Invalid format of the expected message response.");
    }
}
