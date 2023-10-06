﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.UnpublishUrl;

public interface IUnpublishShortenedUrlCommandHandler :
    ICommandHandler<UnpublishShortenedUrlCommand, Result<ShortenedUrlResult>>
{
}
