﻿namespace D20Tek.Authentication.Individual.Api;

internal class Configuration
{
    public class Authentication
    {
        public const string BaseUrl = "/api/v1/account";
        public const string GroupTag = "Authentication";
    }

    public class Register
    {
        public const string RoutePattern = "/";
        public const string EndpointName = "Register";
        public const string DisplayName = "Register";
        public const string CreatedAtUrl = "/api/v1/auth/account";
    }

    public class Login
    {
        public const string RoutePattern = "/login";
        public const string EndpointName = "Login";
        public const string DisplayName = "Login";
    }

    public class ChangePassword
    {
        public const string RoutePattern = "/password";
        public const string EndpointName = "ChangePassword";
        public const string DisplayName = "Change Password";
    }

    public class ChangeRole
    {
        public const string RoutePattern = "/role";
        public const string EndpointName = "ChangeRole";
        public const string DisplayName = "Change Role";
    }

    public class GetAccount
    {
        public const string RoutePattern = "/";
        public const string EndpointName = "GetAccount";
        public const string DisplayName = "Get Account";
    }

    public class RemoveAccount
    {
        public const string RoutePattern = "/";
        public const string EndpointName = "RemoveAccount";
        public const string DisplayName = "Remove Account";
    }

    public class GetClaims
    {
        public const string RoutePattern = "/claims";
        public const string EndpointName = "Get Claims";
        public const string DisplayName = "Get Claims";
    }
}
