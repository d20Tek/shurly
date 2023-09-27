//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.GetById;
using D20Tek.Authentication.Individual.UseCases.RemoveAccount;
using D20Tek.Authentication.Individual.UseCases.UpdateAccount;
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
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
            .RequireAuthorization()
            .WithOpenApi();

        group.MapGet(Configuration.GetAccount.RoutePattern, GetAccountAsync)
            .WithConfiguration(Configuration.GetAccount);

        group.MapPut(Configuration.UpdateAccount.RoutePattern, UpdateAccountAsync)
            .WithConfiguration(Configuration.UpdateAccount);

        group.MapDelete(Configuration.RemoveAccount.RoutePattern, RemoveAccountAsync)
            .WithConfiguration(Configuration.RemoveAccount);
    }

    public async Task<IResult> GetAccountAsync(
        [AsParameters] ClaimsRequest request,
        [FromServices] IGetByIdQueryHandler queryHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new GetByIdQuery(userId);
        var accountResult = await queryHandler.HandleAsync(query, cancellation);

        return accountResult.ToApiResult(_responseMapper.Map);
    }

    public async Task<IResult> UpdateAccountAsync(
        [AsParameters] AuthRequestEnvelope<UpdateAccountRequest> request,
        [FromServices] IUpdateCommandHandler commandHandler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var command = new UpdateCommand(
            userId,
            request.Body.UserName,
            request.Body.GivenName,
            request.Body.FamilyName,
            request.Body.Email,
            request.Body.PhoneNumber);

        var accountResult = await commandHandler.HandleAsync(command, cancellation);

        return accountResult.ToApiResult(_responseMapper.Map);
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
