//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.RefreshToken;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Authentication.Individual.UseCases.ResetPassword;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace D20Tek.Authentication.Individual.Api;

internal class AuthenticationEndpoints : ICompositeApiEndpoint
{
    private readonly AuthenticationResponseMapper _authResponseMapper = new();
    private readonly ResetTokenResponseMapper _resetTokenMapper = new();

    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup(Configuration.Authentication.BaseUrl)
            .WithTags(Configuration.Authentication.GroupTag)
            .WithOpenApi();

        group.MapPost(Configuration.Register.RoutePattern, RegisterAsync)
            .WithConfiguration(Configuration.Register);

        group.MapPost(Configuration.Login.RoutePattern, LoginAsync)
            .WithConfiguration(Configuration.Login);

        group.MapPatch(Configuration.ChangePassword.RoutePattern, ChangePasswordAsync)
            .WithConfiguration(Configuration.ChangePassword);

        group.MapPatch(Configuration.ResetPassword.RoutePattern, ResetPasswordAsync)
            .WithConfiguration(Configuration.ResetPassword);

        group.MapPatch(Configuration.ChangeRole.RoutePattern, ChangeRoleAsync)
            .WithConfiguration(Configuration.ChangeRole)
            .ExcludeFromDescription();

        group.MapPost(Configuration.RefreshToken.RoutePattern, RefreshTokenAsync)
            .WithConfiguration(Configuration.RefreshToken);

        group.MapPost(Configuration.GetResetToken.RoutePattern, GetPasswordResetTokenAsync)
            .WithConfiguration(Configuration.GetResetToken);

        group.MapGet(Configuration.GetClaims.RoutePattern, GetClaims)
            .WithConfiguration(Configuration.GetClaims);
    }

    public async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        [FromServices] IRegisterCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var command = new RegisterCommand(
            request.UserName,
            request.GivenName,
            request.FamilyName,
            request.Email,
            request.Password,
            request.PhoneNumber);

        var authResult = await commandHandler.HandleAsync(command, cancellation);

        return authResult.ToCreatedApiResult(
            _authResponseMapper.Map,
            Configuration.Authentication.CreatedAtUrl);
    }

    public async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] ILoginQueryHandler queryHandler,
        CancellationToken cancellation)
    {
        var query = new LoginQuery(request.UserName, request.Password);
        var authResult = await queryHandler.HandleAsync(query, cancellation);

        return authResult.ToApiResult(_authResponseMapper.Map);
    }

    public async Task<IResult> ChangePasswordAsync(
        [AsParameters] AuthRequestEnvelope<ChangePasswordRequest> request,
        [FromServices] IChangePasswordCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var command = new ChangePasswordCommand(
            userId,
            request.Body.CurrentPassword,
            request.Body.NewPassword);

        var authResult = await commandHandler.HandleAsync(command, cancellation);

        return authResult.ToApiResult(_authResponseMapper.Map);
    }

    public async Task<IResult> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest request,
        [FromServices] IResetPasswordCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var command = new ResetPasswordCommand(
            request.Email,
            request.ResetToken,
            request.NewPassword);

        var authResult = await commandHandler.HandleAsync(command, cancellation);

        return authResult.ToApiResult(_authResponseMapper.Map);
    }

    public async Task<IResult> ChangeRoleAsync(
        [FromBody] ChangeRoleRequest request,
        [FromServices] IChangeRoleCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var command = new ChangeRoleCommand(
            request.UserName,
            request.NewRole);

        var authResult = await commandHandler.HandleAsync(command, cancellation);

        return authResult.ToApiResult();
    }

    public async Task<IResult> RefreshTokenAsync(
        [AsParameters] ClaimsRequest request,
        [FromServices] IRefreshTokenQueryHandler queryHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new RefreshTokenQuery(userId);

        var authResult = await queryHandler.HandleAsync(query, cancellation);

        return authResult.ToApiResult(_authResponseMapper.Map);
    }

    public async Task<IResult> GetPasswordResetTokenAsync(
        [FromBody] GetResetTokenRequest request,
        [FromServices] IGetResetTokenQueryHandler queryHandler,
        CancellationToken cancellation)
    {
        var query = new GetResetTokenQuery(request.Email);
        var tokenResult = await queryHandler.HandleAsync(query, cancellation);

        return tokenResult.ToApiResult(_resetTokenMapper.Map);
    }

    public IResult GetClaims(ClaimsPrincipal user) =>
        Results.Json(user.Claims.Select(x => x.ToString()));
}
