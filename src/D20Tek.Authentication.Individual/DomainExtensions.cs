//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using Res = D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Minimal.Domain;

public static class DomainExtensions
{
}

public static class UseCaseOperation
{
    public static async Task<Result<TResult>> InvokeAsync<TResult>(
        ILogger logger,
        Func<Task<Result<TResult>>> operation,
        [CallerMemberName] string operationName = "handler")
        where TResult : class
    {
        using var scope = logger.BeginScope($"{operationName} method");
        logger.LogInformation($"==> UseCase started: {operationName}.");

        var result = await operation();
        LogResult(logger, operationName, result);

        return result;
    }

    public static async Task<Res.Result> InvokeAsync(
        ILogger logger,
        Func<Task<Res.Result>> operation,
        [CallerMemberName] string operationName = "handler")
    {
        using var scope = logger.BeginScope($"{operationName} method");
        logger.LogInformation($"==> UseCase started: {operationName}.");

        var result = await operation();
        LogResult(logger, operationName, result);

        return result;
    }

    private static void LogResult(ILogger logger, string operationName, Res.Result result)
    {
        if (result.IsSuccess is true)
            logger.LogInformation($"==> Result = {result}");
        else
            logger.LogError($"==> Result = {result}");

        logger.LogInformation($"==> UseCase completed: {operationName}.");
    }
}
