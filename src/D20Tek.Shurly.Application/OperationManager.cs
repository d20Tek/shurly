//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Minimal.Domain;

public class OperationManager
{
    private readonly ILogger _logger;
    private readonly string _className;

    public OperationManager(ILogger logger, string className = "default")
    {
        _logger = logger;
        _className = className;
    }

    public async Task<TResult?> OperationAsync<TResult>(
        Func<Task<TResult>> operation,
        [CallerMemberName] string caller = "method")
    {
        try
        {
            return await operation.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing {_className}.{caller} operation.");
            return default;
        }
    }

    public TResult? Operation<TResult>(
        Func<TResult> operation,
        [CallerMemberName] string caller = "method")
    {
        try
        {
            return operation.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing {_className}.{caller} operation.");
            return default;
        }
    }

    public async Task<TResult> ValueOperationAsync<TResult>(
        Func<Task<TResult>> operation,
        [CallerMemberName] string caller = "method")
        where TResult : struct
    {
        try
        {
            return await operation.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing {_className}.{caller} operation.");
            return default;
        }
    }

    public TResult ValueOperation<TResult>(
        Func<TResult> operation,
        [CallerMemberName] string caller = "method")
        where TResult : struct
    {
        try
        {
            return operation.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing {_className}.{caller} operation.");
            return default;
        }
    }
}
