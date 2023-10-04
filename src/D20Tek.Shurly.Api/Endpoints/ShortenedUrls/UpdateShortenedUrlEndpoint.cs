//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed class UpdateShortenedUrlEndpoint :
    IApiEndpoint<UpdateShortenedUrlRequest, IUpdateShortenedUrlCommandHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut(Configuration.ShortUrl.Update.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.Update);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] UpdateShortenedUrlRequest request,
        [FromServices] IUpdateShortenedUrlCommandHandler handler,
        CancellationToken cancellation)
    {
        var creatorId = request.User.FindUserId();
        var command = new UpdateShortenedUrlCommand(
            request.Id,
            request.Body.LongUrl,
            request.Body.Summary,
            creatorId,
            request.Body.PublishOn);

        var result = await handler.HandleAsync(command, cancellation);
        return result.Match<IResult>(
            success => TypedResults.Ok(_responseMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
