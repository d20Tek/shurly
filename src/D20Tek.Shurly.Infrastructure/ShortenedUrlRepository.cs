//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.Entities.ShortenedUrl;
using Microsoft.EntityFrameworkCore;

namespace D20Tek.Shurly.Infrastructure;

internal class ShortenedUrlRepository : IShortenedUrlRepository
{
    private readonly ShurlyDbContext _dbContext;

    public ShortenedUrlRepository(ShurlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShortenedUrl?> GetByIdAsync(ShortenedUrlId id)
    {
        var entity = await _dbContext.ShortenedUrls
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code)
    {
        var entity = await _dbContext.ShortenedUrls
            .FirstOrDefaultAsync(x => x.ShortUrlCode == code);

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
        // detach the existing entity before applying the updated instance.
        var entityToDetach = _dbContext.ShortenedUrls.Find(shortenedUrl.Id);
        if (entityToDetach is not null)
        {
            var detach = _dbContext.Entry(entityToDetach);
            if (detach.State != EntityState.Detached)
            {
                detach.State = EntityState.Detached;
            }
        }

        _dbContext.ShortenedUrls.Update(shortenedUrl);
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
