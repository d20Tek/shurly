namespace D20Tek.Authentication.Individual.Api;

internal class Configuration
{
    public class Authentication
    {
        public const string BaseUrl = "/api/v1/auth";
        public const string GroupTag = "Authentication";
    }

    public class Register
    {
        public const string RoutePattern = "/register";
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
}
