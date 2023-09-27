//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints.Configuration;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Authentication.Individual.Api;

internal class Configuration
{
    public class Authentication
    {
        public const string BaseUrl = "/api/v1/account";
        public const string GroupTag = "Authentication";
        public const string CreatedAtUrl = "/api/v1/auth/account";
    }

    public static ApiEndpointConfig Register = new(
        "/",
        "Register",
        "Register",
        Summary: "Creates new user account based on data in message body.",
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status201Created),
            Config.ProducesProblem(StatusCodes.Status409Conflict),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
        });

    public static ApiEndpointConfig Login = new(
        "/login",
        "Login",
        "Login",
        Summary: "Logs in exsiting user with user name and password.",
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
        });

    public static ApiEndpointConfig ChangePassword = new(
        "/password",
        "ChangePassword",
        "Change Password",
        Summary: "Changes authenticated user's password.",
        RequiresAuthorization: true,
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status403Forbidden),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });

    public static ApiEndpointConfig ChangeRole = new(
        "/role",
        "ChangeRole",
        "Change Role",
        Summary: "Changes authenticated user's role (user or admin).",
        RequiresAuthorization: true,
        AuthorizationPolicies: new[] { AuthorizationPolicies.Admin },
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesProblem(StatusCodes.Status422UnprocessableEntity),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });

    public static ApiEndpointConfig GetAccount = new (
        "/",
        "GetAccount",
        "Get Account",
        Summary: "Gets account information for logged in user.",
        Produces: new[]
        {
            Config.Produces<AccountResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });

    public static ApiEndpointConfig UpdateAccount = new(
        "/",
        "UpdateAccount",
        "Update Account",
        Summary: "Changes account information for logged in user from message body.",
        Produces: new[]
        {
            Config.Produces<AccountResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status409Conflict),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
        });

    public static ApiEndpointConfig RemoveAccount = new(
        "/",
        "RemoveAccount",
        "Remove Account",
        Summary: "Deletes account information for logged in user.",
        Produces: new[]
        {
            Config.Produces<AccountResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });

    public static ApiEndpointConfig RefreshToken = new(
        "/token/refresh",
        "RefreshToken",
        "Refresh Token",
        Summary: "Refreshes the user's access token with the previous refresh token.",
        RequiresAuthorization: true,
        AuthorizationPolicies: new[] { AuthorizationPolicies.Refresh },
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });

    public static ApiEndpointConfig GetResetToken = new(
        "/password/reset",
        "GetPasswordResetToken",
        "Get Password Reset Token",
        Summary: "Gets the password reset token for a user account.",
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
        });

    public static ApiEndpointConfig ResetPassword = new(
        "/password/reset",
        "ResetPassword",
        "Reset Password",
        Summary: "Resets the user's password with a special token.",
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status403Forbidden),
            Config.ProducesProblem(StatusCodes.Status404NotFound),
            Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
        });

    public static ApiEndpointConfig GetClaims = new(
        "/claims",
        "GetClaims",
        "Get Claims",
        Summary: "Get the logged in user's token claims.",
        RequiresAuthorization: true,
        Produces: new[]
        {
            Config.Produces<AuthenticationResponse>(StatusCodes.Status200OK),
            Config.ProducesProblem(StatusCodes.Status401Unauthorized)
        });
}
