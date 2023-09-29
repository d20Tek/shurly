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

    public async Task<ShortenedUrl?> GetByShortUrlCodeAsync(string shortUrlCode)
    {
        var entity = await _dbContext.ShortenedUrls
            .FirstOrDefaultAsync(x => x.ShortUrlCode.Value == shortUrlCode);

        return entity;
    }

    public async Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode) =>
        await GetByShortUrlCodeAsync(shortUrlCode) is null;

    public async Task<bool> CreateAync(ShortenedUrl shortenedUrl)
    {
        await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
