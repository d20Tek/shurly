﻿//---------------------------------------------------------------------------------------------------------------------
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

        public static Error PropertyEmpty(string name) => Error.Validation(
                $"{name}.Empty",
                $"{name} cannot be empty.");

        public static Error PropertyTooLong(string name) => Error.Validation(
                $"{name}.TooLong",
                $"{name} is too long.");

        public static readonly Error EmailInvalidFormat = Error.Validation(
                "Email.InvalidFormat",
                "Email property is not a valid email format.");

        public static readonly Error PasswordLength = Error.Validation(
                "Password.LengthInvalid",
                "Password must contain between 6 and 32 characters.");

        public static readonly Error EmailAlreadyInUse = Error.Conflict(
                "Email.AlreadyInUse",
                "Email address is already in use and must be unique per account.");

        public static readonly Error UserNameAlreadyInUse = Error.Conflict(
                "UserName.AlreadyInUse",
                "User name is already in use and must be unique per account.");

        public static readonly Error CannotAttachRole = Error.Invalid(
                "UserRole.CannotAttach",
                "Cannot attach the user role to this account.");
    }

    public static class Authentication
    {
        public static readonly Error InvalidCredentials = Error.Validation(
                "Auth.InvalidCredentials",
                "The provided user credentials are not valid.");

        public static readonly Error InvalidExistingCredentials = Error.Validation(
                "Auth.InvalidExistingCredentials",
                "The provided user's existing credentials are not valid.");
    }
}