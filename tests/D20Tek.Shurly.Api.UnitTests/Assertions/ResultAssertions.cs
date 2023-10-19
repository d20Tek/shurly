//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Api.UnitTests.Assertions;

internal static class ResultAssertions
{
    public static void ShouldBeFailure(
        this Result result, params Error[] expectedErrors)
    {
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.ValueOrDefault.Should().BeNull();
        result.Errors.Should().NotBeNull();

        foreach (var error in expectedErrors)
        {
            result.Errors.Should().Contain(error);
        }
    }
}
