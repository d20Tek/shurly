//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Web.Helpers;

namespace D20Tek.Shurly.Web.Services.Contracts;

public sealed record ShortenedUrlResponse(
    string Id,
    string Title,
    string LongUrl,
    string ShortUrl,
    string Summary,
    List<string> Tags,
    int ClickCount,
    UrlState State,
    DateTime CreatedOn,
    DateTime PublishOn)
{
    internal UpdateShortenedUrlRequest ToUpdateRequest()
    {
        return new UpdateShortenedUrlRequest
        {
            Title = this.Title,
            LongUrl = this.LongUrl,
            Summary = this.Summary,
            Tags = this.Tags,
            TagsRaw = string.Join("; ", this.Tags),
            PublishOn = this.PublishOn,
            HasPublishDate = this.PublishOn > DateTime.UtcNow,
            LocalPublishOn = DateTimeHelper.UtcToLocalDateTime(this.PublishOn)
        };
    }
}
