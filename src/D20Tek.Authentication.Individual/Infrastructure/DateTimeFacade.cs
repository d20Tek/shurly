//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class DateTimeFacade : IDateTimeFacade
{
    public DateTime UtcNow => DateTime.UtcNow;
}
