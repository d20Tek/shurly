//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class ShortUrlCode : ValueObject
{
    public const int MaxLength = 8;

    public string Value { get; }

    public ShortUrlCode(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static ShortUrlCode Create(string code) => new ShortUrlCode(code);
}
