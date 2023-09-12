//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual;

internal class Errors
{
    public static class UserAccount
    {
        public static Error NotFound(Guid accountId) => Error.NotFound(
                "Account.NotFound",
                $"The account with Id [{accountId}] was not found.");

        public static readonly Error IdInvalid = Error.Validation(
                "AccountId.Empty",
                "The specified account id is invalid.");

        public static readonly Error GivenNameEmpty = Error.Validation(
                "GivenName.Empty",
                "Given name cannot be empty.");

        public static readonly Error GivenNameTooLong = Error.Validation(
                "GivenName.TooLong",
                "Given name is too long.");

        public static readonly Error FamilyNameEmpty = Error.Validation(
                "Family Name.Empty",
                "Family name cannot be empty.");

        public static readonly Error FamilyNameTooLong = Error.Validation(
                "Family.TooLong",
                "Family name is too long.");

        public static readonly Error EmailEmpty = Error.Validation(
                "Email.Empty",
                "Email cannot be empty.");

        public static readonly Error EmailTooLong = Error.Validation(
                "Email.TooLong",
                "Email is too long.");

        public static readonly Error EmailInvalidFormat = Error.Validation(
                "Email.InvalidFormat",
                "Email property is not a valid email format.");

        public static readonly Error PasswordEmpty = Error.Validation(
                "Password.Empty",
                "Password cannot be empty.");

        public static readonly Error PasswordLength = Error.Validation(
                "Password.LengthInvalid",
                "Password must contain between 6 and 32 characters.");

        public static readonly Error EmailAlreadyInUse = Error.Conflict(
                "Email.AlreadyInUse",
                "Email address is already in use and must be unique per account.");

        public static readonly Error UserNameAlreadyInUse = Error.Conflict(
                "UserName.AlreadyInUse",
                "User name is already in use and must be unique per account.");
    }
}
