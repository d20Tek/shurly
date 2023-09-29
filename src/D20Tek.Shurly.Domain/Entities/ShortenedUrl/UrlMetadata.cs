//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.Entities.ShortenedUrl;

public sealed class UrlMetadata : ValueObject
{
    public const int MaxLength = 8;

    public UrlState State { get; private set; }

    public AccountId OwnerId { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public DateTime PublishOn { get; private set; }

    public DateTime ModifiedOn { get; private set; }

    private UrlMetadata(
        UrlState state,
        AccountId ownerId,
        DateTime createdOn,
        DateTime publishOn,
        DateTime modifiedOn)
    {
        State = state;
        OwnerId = ownerId;
        CreatedOn = createdOn;
        PublishOn = publishOn;
        ModifiedOn = modifiedOn;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return State;
        yield return OwnerId;
        yield return CreatedOn;
        yield return PublishOn;
        yield return ModifiedOn;
    }

    public void PublishUrl()
    {
        var now = DateTime.UtcNow;
        State = UrlState.Published;
        PublishOn = now;
        ModifiedOn = now;
    }

    public void UnpublishUrl()
    {
        State = UrlState.Obsolete;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Modified()
    {
        ModifiedOn = DateTime.UtcNow;
    }

    public static UrlMetadata Create(AccountId creatorId)
    {
        return new UrlMetadata(
            UrlState.New,
            creatorId,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public static UrlMetadata Hydrate(
        UrlState state,
        AccountId creatorId,
        DateTime createdOn,
        DateTime publishOn,
        DateTime modifiedOn)
    {
        return new(state, creatorId, createdOn, publishOn, modifiedOn);
    }
}
