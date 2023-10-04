//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.EntityFrameworkCore;

namespace D20Tek.Shurly.Infrastructure;

internal class ShortenedUrlRepository : IShortenedUrlRepository
{
    private readonly ShurlyDbContext _dbContext;

    public ShortenedUrlRepository(ShurlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<ShortenedUrl>> GetForOwnerAsync(AccountId ownerId)
    {
        var entities = await _dbContext.ShortenedUrls
            .Where(x => x.UrlMetadata.OwnerId == ownerId)
            .OrderBy(x => x.UrlMetadata.CreatedOn)
            .ToListAsync();

        return entities;
    }

    public async Task<ShortenedUrl?> GetByIdAsync(ShortenedUrlId id)
    {
        var entity = await _dbContext.ShortenedUrls
            .SingleOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code)
    {
        var entity = await _dbContext.ShortenedUrls
            .SingleOrDefaultAsync(
                x => x.ShortUrlCode == code && x.UrlMetadata.State == UrlState.Published);

        return entity;
    }

    public async Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode) =>
        await GetByShortUrlCodeAsync(ShortUrlCode.Create(shortUrlCode)) is null;

    public async Task<bool> CreateAync(ShortenedUrl shortenedUrl)
    {
        await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(ShortenedUrl shortenedUrl)
    {
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(ShortenedUrl shortenedUrl)
    {
        _dbContext.ShortenedUrls.Remove(shortenedUrl);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
