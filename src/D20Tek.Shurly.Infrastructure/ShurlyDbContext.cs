//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;
using Microsoft.EntityFrameworkCore;

namespace D20Tek.Shurly.Infrastructure;

internal class ShurlyDbContext : DbContext
{
    public ShurlyDbContext(DbContextOptions<ShurlyDbContext> options)
        : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ShortenedUrlId.Create(value));

            builder.Property(x => x.LongUrl)
                .HasMaxLength(LongUrl.MaxLength)
                .HasConversion(
                    url => url.Value,
                    value => LongUrl.Create(value));

            builder.Property(x => x.Title)
                .HasMaxLength(Title.MaxLength)
                .HasConversion(
                    title => title.Value,
                    value => Title.Create(value));

            builder.Property(x => x.Summary)
                .HasMaxLength(Summary.MaxLength)
                .HasConversion(
                    summary => summary.Value,
                    value => Summary.Create(value));

            builder.Property(x => x.ShortUrlCode)
                .HasMaxLength(UrlShorteningService.NumCharsInCode)
                .HasConversion(
                    code => code.Value,
                    value => ShortUrlCode.Create(value));

            builder.HasIndex(x => x.ShortUrlCode).IsUnique();

            builder.OwnsOne<UrlMetadata>(x => x.UrlMetadata, b =>
            {
                b.Property(x => x.OwnerId)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => AccountId.Create(value));

                b.HasIndex(x => x.OwnerId);

                b.Property(x => x.Tags)
                    .HasConversion(
                        tags => string.Join(';', tags),
                        value => value.Split(";", StringSplitOptions.None).ToList());
            });
        });

        modelBuilder.FinalizeModel();
    }
}
