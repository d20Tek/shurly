//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class LongUrl : ValueObject
{
    public const int MaxLength = 2048;

    public string Value { get; }

    public LongUrl(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static LongUrl Create(string longUrl) => new LongUrl(longUrl);
}
