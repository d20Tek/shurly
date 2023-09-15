//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Minimal.Endpoints;
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

    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup(Configuration.Authentication.BaseUrl)
            .WithTags(Configuration.Authentication.GroupTag)
            .WithOpenApi();

        group.MapPost(Configuration.Register.RoutePattern, RegisterAsync)
            .WithName(Configuration.Register.EndpointName)
            .WithDisplayName(Configuration.Register.DisplayName)
            .Produces<AuthenticationResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPost(Configuration.Login.RoutePattern, LoginAsync)
            .WithName(Configuration.Login.EndpointName)
            .WithDisplayName(Configuration.Login.DisplayName)
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPatch(Configuration.ChangePassword.RoutePattern, ChangePasswordAsync)
            .WithName(Configuration.ChangePassword.EndpointName)
            .WithName(Configuration.ChangePassword.DisplayName)
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();

        group.MapGet(Configuration.GetClaims.RoutePattern, GetClaims)
            .WithName(Configuration.GetClaims.EndpointName)
            .WithDisplayName(Configuration.GetClaims.DisplayName)
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization();
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
            request.Password);

        var authResult = await commandHandler.HandleAsync(command, cancellation);

        return authResult.ToCreatedApiResult(
            _authResponseMapper.Map,
            Configuration.Register.CreatedAtUrl);
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

    public IResult GetClaims(ClaimsPrincipal user) =>
        Results.Json(user.Claims.Select(x => x.ToString()));
}
