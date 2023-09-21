//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client;

internal static class Configuration
{
    public class Authentication
    {
        public const string ChangePassword = "/password";
        public const string ResetPassword = "/password/reset";
        public const string Delete = "/";
        public const string Get = "/";
        public const string Login = "/login";
        public const string Register = "/";
        public const string Update = "/";

        public const string JwtBearerScheme = "Bearer";
        public const string AccessTokenKey = "accessToken";
        public const string RefreshTokenKey = "refreshToken";
    }
}
