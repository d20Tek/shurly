//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Identity;

namespace D20Tek.Authentication.Individual.UseCases;

internal static class IdentityResultExtensions
{
    public static Result<TResult> ToMinimalResult<TResult>(this IdentityResult result)
    {
        var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
        return errors;
    }
}
