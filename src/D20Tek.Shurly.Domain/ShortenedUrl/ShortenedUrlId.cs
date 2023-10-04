//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class ShortenedUrlId : GuidId
{
    public ShortenedUrlId(Guid value)
        : base(value)
    {
    }

    public static ShortenedUrlId Create() => new(Guid.NewGuid());

    public static ShortenedUrlId Create(Guid guid) => new(guid);
}
