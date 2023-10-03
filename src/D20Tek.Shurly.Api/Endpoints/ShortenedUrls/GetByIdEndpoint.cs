//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetById;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed class GetByIdEndpoint :
    IApiEndpoint<GetByIdRequest, IGetByIdQueryHandler>

{
    private readonly ShortenedUrlResponseMapper _responseMapper = new();

    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(Configuration.ShortUrl.GetById.RoutePattern, HandleAsync)
            .WithConfiguration(Configuration.ShortUrl.GetById);
    }

    public async Task<IResult> HandleAsync(
        [AsParameters] GetByIdRequest request,
        [FromServices] IGetByIdQueryHandler handler,
        CancellationToken cancellation)
    {
        var userId = request.User.FindUserId();
        var query = new GetByIdQuery(request.Id, userId);
        var result = await handler.HandleAsync(query, cancellation);

        return result.Match<IResult>(
            success => TypedResults.Ok(_responseMapper.Map(success, request.Context)),
            errors => Results.Extensions.Problem(errors));
    }
}
