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
    private readonly ILogger _logger;

    public ShortenedUrlRepository(
        ShurlyDbContext dbContext,
        ILogger<ShortenedUrlRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IList<ShortenedUrl>> GetForOwnerAsync(AccountId ownerId)
    {
        return await OperationLogManager.OperationAsync<IList<ShortenedUrl>>(async () =>
        {
            var entities = await _dbContext.ShortenedUrls
            .Where(x => x.UrlMetadata.OwnerId == ownerId)
            .OrderByDescending(x => x.UrlMetadata.CreatedOn)
            .ToListAsync();

            return entities;
        }, _logger) ?? Array.Empty<ShortenedUrl>();
    }

    public async Task<ShortenedUrl?> GetByIdAsync(ShortenedUrlId id)
    {
        return await OperationLogManager.OperationAsync<ShortenedUrl?>(async () =>
        {
            var entity = await _dbContext.ShortenedUrls
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity;
        }, _logger);
    }

    public async Task<ShortenedUrl?> GetByShortUrlCodeAsync(ShortUrlCode code)
    {
        return await OperationLogManager.OperationAsync<ShortenedUrl?>(async () =>
        {
            var entity = await _dbContext.ShortenedUrls
            .FirstOrDefaultAsync(
                x => x.ShortUrlCode == code && x.UrlMetadata.State == UrlState.Published);

            return entity;
        }, _logger);
    }

    public async Task<bool> IsUrlCodeUniqueAsync(string shortUrlCode) =>
        await GetByShortUrlCodeAsync(ShortUrlCode.Create(shortUrlCode)) is null;

    public async Task<bool> CreateAync(ShortenedUrl shortenedUrl)
    {
        return await OperationLogManager.ValueOperationAsync<bool>(async () =>
        {
            await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
            var itemsSaved = await _dbContext.SaveChangesAsync();
            return itemsSaved > 0;
        }, _logger);
    }

    public async Task<bool> UpdateAsync(ShortenedUrl shortenedUrl)
    {
        return await OperationLogManager.ValueOperationAsync<bool>(async () =>
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }, _logger);
    }

    public async Task<bool> DeleteAsync(ShortenedUrl shortenedUrl)
    {
        return await OperationLogManager.ValueOperationAsync<bool>(async () =>
        {
            _dbContext.ShortenedUrls.Remove(shortenedUrl);
            var itemsSaved = await _dbContext.SaveChangesAsync();
            return itemsSaved > 0;
        }, _logger);
    }
}
