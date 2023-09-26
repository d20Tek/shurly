//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using Res = D20Tek.Minimal.Result;
using System.Net.Http.Json;
using D20Tek.Minimal.Result.Client;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Authentication.Individual.Client;

internal abstract class ServiceBase
{
    protected readonly ILogger Logger;

    protected ServiceBase(ILogger logger)
    {
        Logger = logger;
    }

    protected async Task<Result<T>> InvokeServiceOperation<T>(
        Func<Task<HttpResponseMessage>> operation,
        Func<Result<T>, Task>? onSuccess = null,
        [CallerMemberName] string operationName = "operation")
        where T : class
    {
        try
        {
            using var scope = Logger.BeginScope($"{operationName} method");
            Logger.LogInformation($"==> Service call started: {operationName}.");
            HttpResponseMessage result = await operation();

            var response = await ProcessHttpResponse<T>(result);
            if (onSuccess is not null)
            {
                await onSuccess(response);
            }

            LogResult(operationName, response);
            return response;
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"==> Service call failed: {ex}.", ex);
            return ex;
        }
    }

    protected async Task<Res.Result> InvokeOperation(
        Func<Task> operation,
        [CallerMemberName] string operationName = "operation")
    {
        try
        {
            using var scope = Logger.BeginScope($"{operationName} method");
            Logger.LogInformation($"==> Service call started: {operationName}.");

            await operation();

            Logger.LogInformation($"==> Service call completed: {operationName}.");
            return Res.Result.Success();
        }
        catch (Exception ex)
        {
            Logger.LogInformation($"==> Service call failed: {ex}.", ex);
            return ex;
        }
    }

    protected async Task<Result<T>> ProcessHttpResponse<T>(HttpResponseMessage message)
        where T : class
    {
        if (message.IsSuccessStatusCode is false)
        {
            var problem = await message.Content.ReadFromJsonAsync<ProblemDetails>();
            if (problem is not null)
            {
                var result = problem.ToResult<T>();
                Console.WriteLine("AuthenticationService error:");
                Console.WriteLine(problem.ToString());

                return result;
            }

            return Errors.AuthenticationService.CannotParseProblem;
        }

        var response = await message.Content.ReadFromJsonAsync<T>();
        if (response is null)
        {
            return Errors.AuthenticationService.CannotConvertPayload;
        }

        return response;
    }

    private void LogResult(string operationName, Res.Result result)
    {
        if (result.IsSuccess is true)
            Logger.LogInformation($"==> Result = {result}");
        else
            Logger.LogError($"==> Result = {result}", result);

        Logger.LogInformation($"==> Service call completed: {operationName}.");
    }
}
