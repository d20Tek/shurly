﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
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
    DateTime PublishOn);
