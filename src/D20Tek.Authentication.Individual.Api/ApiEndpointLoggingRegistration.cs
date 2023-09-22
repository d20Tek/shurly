//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;

namespace D20Tek.Minimal.Endpoints;

public static class ApiEndpointLoggingRegistration
{
    private static ApiEndpointLoggingFilter _endpointLogger = new();

    public static IApplicationBuilder UseApiEndpointLogging(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
            await _endpointLogger.InvokeAsync(context, next));

        return app;
    }
}
