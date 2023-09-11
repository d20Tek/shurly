//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IDateTimeFacade
{
    public DateTime UtcNow { get; }
}
