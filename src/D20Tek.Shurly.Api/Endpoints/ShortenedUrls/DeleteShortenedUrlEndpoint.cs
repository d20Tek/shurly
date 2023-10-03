//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Delete;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

public class DeleteShortenedUrlEndpoint :
    IApiEndpoint<DeleteShortenedUrlRequest, IDeleteShortenedUrlCommandHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete(Configuration.ShortUrl.Delete.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.Delete);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] DeleteShortenedUrlRequest request,
        [FromServices] IDeleteShortenedUrlCommandHandler handler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new DeleteShortenedUrlCommand(request.Id, userId);
        var result = await handler.HandleAsync(query, cancellation);

        return result.Match<IResult>(
            success => TypedResults.Ok(_responseMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
