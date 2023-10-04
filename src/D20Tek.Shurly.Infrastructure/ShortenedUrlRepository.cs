//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain;
using D20Tek.Shurly.Application.Abstractions;
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace D20Tek.Shurly.Infrastructure;

internal class ShortenedUrlRepository : IShortenedUrlRepository
{
    private readonly ShurlyDbContext _dbContext;
    private readonly OperationManager _opsManager;

    public ShortenedUrlRepository(
        ShurlyDbContext dbContext,
        ILogger<ShortenedUrlRepository> logger)
    {
        _dbContext = dbContext;
        _opsManager = new OperationManager(logger, nameof(ShortenedUrlRepository));
    }

    public async Task<IList<ShortenedUrl>> GetForOwnerAsync(AccountId ownerId)
    {
        return await _opsManager.OperationAsync<IList<ShortenedUrl>>(async () =>
        {
            var entities = await _dbContext.ShortenedUrls
            .Where(x => x.UrlMetadata.OwnerId == ownerId)
            .OrderBy(x => x.UrlMetadata.CreatedOn)
            .ToListAsync();

            return entities;
        }) ?? Array.Empty<ShortenedUrl>();
    }

    public async Task<ShortenedUrl?> GetByIdAsync(ShortenedUrlId id)
    {
        return await _opsManager.OperationAsync<ShortenedUrl?>(async () =>
        {
            var entity = await _dbContext.ShortenedUrls
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity;
        });
    }

    public async Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code)
    {
        return await _opsManager.OperationAsync<ShortenedUrl?>(async () =>
        {
            var entity = await _dbContext.ShortenedUrls
            .SingleOrDefaultAsync(
                x => x.ShortUrlCode == code && x.UrlMetadata.State == UrlState.Published);

            return entity;
        });
    }

    public async Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode) =>
        await GetByShortUrlCodeAsync(ShortUrlCode.Create(shortUrlCode)) is null;

    public async Task<bool> CreateAync(ShortenedUrl shortenedUrl)
    {
        return await _opsManager.ValueOperationAsync<bool>(async () =>
        {
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            var itemsSaved = await _dbContext.SaveChangesAsync();
            return itemsSaved > 0;
        });
    }

    public async Task<bool> UpdateAsync(ShortenedUrl shortenedUrl)
    {
        return await _opsManager.ValueOperationAsync<bool>(async () =>
        {
            await _dbContext.SaveChangesAsync();
            return true;
        });
    }

    public async Task<bool> DeleteAsync(ShortenedUrl shortenedUrl)
    {
        return await _opsManager.ValueOperationAsync<bool>(async () =>
        {
            _dbContext.ShortenedUrls.Remove(shortenedUrl);
            var itemsSaved = await _dbContext.SaveChangesAsync();
            return itemsSaved > 0;
        });
    }
}
