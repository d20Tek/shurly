//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.UnpublishUrl;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class UnpublishShortenedUrlEndpoint :
    IApiEndpoint<UnpublishShortenedUrlRequest, IUnpublishShortenedUrlCommandHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut(Configuration.ShortUrl.Unpublish.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.Unpublish)
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] UnpublishShortenedUrlRequest request,
        [FromServices] IUnpublishShortenedUrlCommandHandler handler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var command = new UnpublishShortenedUrlCommand(request.Id, userId);

        var result = await handler.HandleAsync(command, cancellation);
        return result.Match<IResult>(
            success => TypedResults.Ok(_responseMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
