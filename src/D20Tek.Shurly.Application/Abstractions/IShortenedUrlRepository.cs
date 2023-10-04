//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.Abstractions;

public interface IShortenedUrlRepository
{
    public Task<IList<ShortenedUrl>> GetForOwnerAsync(AccountId ownerId);

    public Task<ShortenedUrl?> GetByIdAsync(ShortenedUrlId id);

    public Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code);

    public Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode);

    public Task<bool> CreateAync(ShortenedUrl shortenedUrl);

    public Task<bool> UpdateAsync(ShortenedUrl shortenedUrl);

    public Task<bool> DeleteAsync(ShortenedUrl shortenedUrl);
}
