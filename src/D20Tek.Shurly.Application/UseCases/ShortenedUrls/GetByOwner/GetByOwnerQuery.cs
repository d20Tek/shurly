﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.GetByOwner;

public sealed record GetByOwnerQuery(
    Guid OwnerId,
    int Skip,
    int Take) : IQuery<Result<ShortenedUrlList>>;
