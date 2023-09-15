//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class OperationManager
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

    public async Task<IdentityResult> OperationAsync(
        Func<Task<IdentityResult>> operation,
        [CallerMemberName] string caller = "method")
    {
        try
        {
            var result = await operation.Invoke();
            if (result.Succeeded is false)
            {
                _logger.LogError($"{_className}.{caller} failed: {result.Errors.First()}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing {_className}.{caller} operation.");

            var result = new IdentityResult();
            var error = new IdentityError { Code = "Unexpected", Description = ex.Message };
            result.Errors.Append(error);

            return result;
        }
    }
}
