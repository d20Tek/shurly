//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByShortCode;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class GetByShortCodeEndpoint :
    IApiEndpoint<GetByShortCodeRequest, IGetByShortCodeQueryHandler>
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(Configuration.ShortUrl.GetByShortCode.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.GetByShortCode);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] GetByShortCodeRequest request,
        [FromServices] IGetByShortCodeQueryHandler handler,
        CancellationToken cancellation)
    {
        var query = new GetByShortCodeQuery(request.shortCode);
        var result = await handler.HandleAsync(query, cancellation);

        return result.Match<IResult>(
            success => TypedResults.Redirect(success.LongUrl),
            errors => Results.Extensions.Problem(errors));
    }
}
