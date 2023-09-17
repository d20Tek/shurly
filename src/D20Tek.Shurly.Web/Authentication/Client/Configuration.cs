//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client;

internal static class Configuration
{
    public class Authentication
    {
        public const string BaseUrl = "/api/v1/account";
        public const string Login = "/login";

        public const string JwtBearerScheme = "Bearer";
        public const string AccessTokenKey = "accessToken";
    }
}
