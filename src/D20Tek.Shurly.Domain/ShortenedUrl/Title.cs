//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class Title : ValueObject
{
    public const int MaxLength = 32;

    public string Value { get; }

    public Title(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Title Create(string title) => new Title(title);
}
