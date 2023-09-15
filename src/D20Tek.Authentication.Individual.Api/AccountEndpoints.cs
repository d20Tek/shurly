//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.GetById;
using D20Tek.Authentication.Individual.UseCases.RemoveAccount;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.Authentication.Individual.Api;

internal class AccountEndpoints : ICompositeApiEndpoint
{
    private readonly AccountResponseMapper _responseMapper = new();

    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup(Configuration.Authentication.BaseUrl)
            .WithTags(Configuration.Authentication.GroupTag)
            .WithOpenApi();

        group.MapGet(Configuration.GetAccount.RoutePattern, GetAccountAsync)
            .WithName(Configuration.GetAccount.EndpointName)
            .WithDisplayName(Configuration.GetAccount.DisplayName)
            .Produces<AccountResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        group.MapDelete(Configuration.RemoveAccount.RoutePattern, RemoveAccountAsync)
            .WithName(Configuration.RemoveAccount.EndpointName)
            .WithDisplayName(Configuration.RemoveAccount.DisplayName)
            .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    public async Task<IResult> GetAccountAsync(
        [AsParameters] ClaimsRequest request,
        [FromServices] IGetByIdQueryHandler queryHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new GetByIdQuery(userId);
        var authResult = await queryHandler.HandleAsync(query, cancellation);

        return authResult.ToApiResult(_responseMapper.Map);
    }

    public async Task<IResult> RemoveAccountAsync(
        [AsParameters] ClaimsRequest request,
        [FromServices] IRemoveCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var command = new RemoveCommand(userId);
        var accountResult = await commandHandler.HandleAsync(command, cancellation);

        return accountResult.ToApiResult(_responseMapper.Map);
    }
}
