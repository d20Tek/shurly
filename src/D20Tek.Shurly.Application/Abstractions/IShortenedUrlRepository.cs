//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;

namespace D20Tek.Shurly.Application.Abstractions;

public interface IShortenedUrlRepository
{
    public Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code);

    public Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode);

    public Task<bool> CreateAync(ShortenedUrl shortenedUrl);
}
