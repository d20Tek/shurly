//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.PublishUrl;

public interface IPublishShortenedUrlCommandHandler :
    ICommandHandler<PublishShortenedUrlCommand, Result<ShortenedUrlResult>>
{
}
