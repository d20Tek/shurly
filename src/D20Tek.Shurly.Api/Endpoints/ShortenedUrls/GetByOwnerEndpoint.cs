//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class GetByOwnerEndpoint : IApiEndpoint<GetByOwnerRequest, IGetByOwnerQueryHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();
    private readonly ShortenedUrlListResponseMapper _listMapper;

    public GetByOwnerEndpoint()
    {
        _listMapper = new(_responseMapper);
    }

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(Configuration.ShortUrl.GetByOwner.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.GetByOwner)
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] GetByOwnerRequest request,
        [FromServices] IGetByOwnerQueryHandler handler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new GetByOwnerQuery(userId, request.Skip, request.Take);
        var result = await handler.HandleAsync(query, cancellation);

        return result.Match<IResult>(
            success => TypedResults.Ok(_listMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
