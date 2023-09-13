//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.Authentication.Individual.Api;

internal class AuthenticationEndpoints : ICompositeApiEndpoint
{
    private readonly AuthenticationResponseMapper _authResponseMapper = new();

    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/v1/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .Produces<AuthenticationResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
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

        return authResult.ToCreatedApiResult(_authResponseMapper.Map, $"/api/v1/auth/account");
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
}
