//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.Entities.ShortenedUrl;

public sealed class AccountId : GuidId
{
    public AccountId(Guid value)
        : base(value)
    {
    }

    public static AccountId Create() => new(Guid.NewGuid());

    public static AccountId Create(Guid guid) => new(guid);
}
