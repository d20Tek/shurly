//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;

namespace D20Tek.Shurly.Domain.ShortenedUrl;

public sealed class UrlMetadata : ValueObject
{
    public const int MaxLength = 8;

    public UrlState State { get; private set; }

    public AccountId OwnerId { get; private set; }

    public List<string> Tags { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public DateTime PublishOn { get; private set; }

    public DateTime ModifiedOn { get; private set; }

    private UrlMetadata(
        UrlState state,
        AccountId ownerId,
        List<string> tags,
        DateTime createdOn,
        DateTime publishOn,
        DateTime modifiedOn)
    {
        State = state;
        OwnerId = ownerId;
        Tags = tags;
        CreatedOn = createdOn;
        PublishOn = publishOn;
        ModifiedOn = modifiedOn;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return State;
        yield return OwnerId;
        yield return Tags;
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

    public void UpdatePublishOn(DateTime publishOn)
    {
        if (publishOn <= DateTime.UtcNow)
        {
            PublishUrl();
        }
        else
        {
            State = UrlState.New;
            PublishOn = publishOn;
            ModifiedOn = DateTime.UtcNow;
        }
    }

    public static UrlMetadata Create(
        AccountId creatorId,
        List<string>? tags = null,
        DateTime? publishOn = null)
    {
        var now = DateTime.UtcNow;
        var newState = UrlState.New;
        var newPublishOn = publishOn ?? now;

        // see if the shortened url should be auto-published
        if (ShouldEntityAutoPublish(publishOn))
        {
            newState = UrlState.Published;
            newPublishOn = now;
        }

        return new UrlMetadata(
            newState,
            creatorId,
            tags ?? new List<string>(),
            now,
            newPublishOn,
            now);
    }

    public static UrlMetadata Hydrate(
        UrlState state,
        AccountId creatorId,
        List<string> tags,
        DateTime createdOn,
        DateTime publishOn,
        DateTime modifiedOn)
    {
        return new(state, creatorId, tags, createdOn, publishOn, modifiedOn);
    }

    private static bool ShouldEntityAutoPublish(DateTime? publishOn) =>
        publishOn is null || publishOn < DateTime.UtcNow;
}
