//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class GetByOwnerEndpoint : IApiEndpoint<HttpContextRequest, IGetByOwnerQueryHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(Configuration.ShortUrl.GetByOwner.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.GetByOwner);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] HttpContextRequest request,
        [FromServices] IGetByOwnerQueryHandler handler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new GetByOwnerQuery(userId);
        var result = await handler.HandleAsync(query, cancellation);

        return result.Match<IResult>(
            success => TypedResults.Ok(
                success.Select(x => _responseMapper.Map(x, request.Context))
                       .ToList()),
            errors => Results.Extensions.Problem(errors));
    }
}
