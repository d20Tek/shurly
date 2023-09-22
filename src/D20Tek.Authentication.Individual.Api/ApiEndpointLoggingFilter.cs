//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace D20Tek.Minimal.Endpoints;

public class ApiEndpointLoggingFilter
{
    public virtual async ValueTask<object?> InvokeAsync(HttpContext context, Func<Task> next)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        ArgumentNullException.ThrowIfNull(nameof(next));

        // capture the request body (if present) for logging
        string requestBody = await ReadRequestBodyAsync(context.Request);
        context.Request.EnableBuffering();

        // read response body (if present) for logging
        var responseBody = await ReadResponseBodyAsync(context, next);

        // log the request and response
        LogRequestResponse(context, requestBody, responseBody);
        return true;
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        request.Body.Seek(0, SeekOrigin.Begin);

        return Encoding.UTF8.GetString(buffer);
    }

    private async Task<string> ReadResponseBodyAsync(HttpContext context, Func<Task> next)
    {
        var responseBody = string.Empty;
        var originalBody = context.Response.Body;

        try
        {
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await next();

            memStream.Position = 0;
            responseBody = new StreamReader(memStream).ReadToEnd();

            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);

        }
        finally
        {
            context.Response.Body = originalBody;
        }

        return responseBody;
    }

    private void LogRequestResponse(HttpContext context, string requestBody, string responseBody)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<ApiEndpointLoggingFilter>>();
        logger.LogInformation("=== New Endpoint Call ================");
        logger.LogInformation(
            $"Request: {context.Request.Method} " +
            $"{context.Request.Path}" +
            $"{context.Request.QueryString}");
        logger.LogInformation($"Request Body: {requestBody}");
        logger.LogInformation($"Response StatusCode: {context.Response.StatusCode}");
        logger.LogInformation($"Response Body: {responseBody}");
    }
}
