//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Shurly.Api.Endpoints.Home;

internal class HomeEndpoints : ICompositeApiEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/", GetHome)
            .ExcludeFromDescription();
    }

    internal string GetHome() => "Shurly Api - Link shortening and management";
}
