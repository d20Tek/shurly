//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.Entities.ShortenedUrl;

public sealed class Summary : ValueObject
{
    public const int MaxLength = 1024;

    public string Value { get; }

    public Summary(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Summary Create(string summary) => new Summary(summary);
}
