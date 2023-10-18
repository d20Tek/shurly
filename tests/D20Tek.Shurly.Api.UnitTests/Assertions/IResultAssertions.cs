//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace D20Tek.Shurly.Api.UnitTests.Assertions;

internal static class IResultAssertions
{
    public static void ShouldBeProblemDetails(
        this IResult result,
        int statusCode = StatusCodes.Status500InternalServerError)
    {
        result.Should().NotBeNull();
        var problem = result.As<ProblemHttpResult>();
        problem.Should().NotBeNull();
        problem.StatusCode.Should().Be(statusCode);
    }
}
