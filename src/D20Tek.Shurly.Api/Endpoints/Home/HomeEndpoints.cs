//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Shurly.Api.Endpoints.Home;

internal class HomeEndpoints : ICompositeApiEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/")
            .ExcludeFromDescription();

        group.MapGet("/", GetHome);

        group.MapGet("/user", GetAuthenticatedUser)
            .RequireAuthorization();

        group.MapGet("/admin", GetAuthenticatedAdmin)
            .RequireAuthorization("AuthZAdmin");

        group.MapGet("/refresh", GetRefreshToken)
            .RequireAuthorization("AuthZRefresh");
    }

    internal string GetHome() => "Shurly Api - Link shortening and management";

    internal string GetAuthenticatedUser() =>
        "Authenticated user accessed this api.";

    internal string GetAuthenticatedAdmin() =>
        "Authenticated admin accessed this api.";

    internal string GetRefreshToken() =>
        "Authenticated refresh policy accessed this api.";
}
