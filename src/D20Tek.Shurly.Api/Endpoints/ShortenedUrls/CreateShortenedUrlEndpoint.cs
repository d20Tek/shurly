//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class CreateShortenedUrlEndpoint :
    IApiEndpoint<HttpContextEnvelope<CreateShortenedUrlRequest>>
{
    private readonly ICreateShortenedUrlCommandHandler _handler;
    private readonly ShortenedUrlResponseMapper _responseMapper;

    public CreateShortenedUrlEndpoint(ICreateShortenedUrlCommandHandler handler)
    {
        _handler = handler;
        _responseMapper = new ShortenedUrlResponseMapper();
    }

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost(Configuration.ShortUrl.Create.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.Create);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] HttpContextEnvelope<CreateShortenedUrlRequest> request,
        CancellationToken cancellation)
    {
        var creatorId = request.User.FindUserId();
        var command = new CreateShortenedUrlCommand(
            request.Body.LongUrl,
            request.Body.Summary,
            creatorId,
            request.Body.PublishOn);

        var result = await _handler.HandleAsync(command, cancellation);
        return result.Match<IResult>(
            success => TypedResults.Ok(_responseMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
