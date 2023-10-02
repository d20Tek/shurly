//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class CreateShortenedUrlEndpoint :
    IApiEndpoint<HttpContextEnvelope<CreateShortenedUrlRequest>, ICreateShortenedUrlCommandHandler>
{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(Configuration.ShortUrl.Create.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.Create);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] HttpContextEnvelope<CreateShortenedUrlRequest> request,
        [FromServices] ICreateShortenedUrlCommandHandler handler,
        CancellationToken cancellation)
    {
        var creatorId = request.User.FindUserId();
        var command = new CreateShortenedUrlCommand(
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
