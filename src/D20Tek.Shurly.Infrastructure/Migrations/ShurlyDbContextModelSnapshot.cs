﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using D20Tek.Shurly.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace D20Tek.Shurly.Infrastructure.Migrations
{
    [DbContext(typeof(ShurlyDbContext))]
    [ExcludeFromCodeCoverage]
    partial class ShurlyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("D20Tek.Shurly.Domain.ShortenedUrl.ShortenedUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("ShortUrlCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("ShortUrlCode")
                        .IsUnique();

                    b.ToTable("ShortenedUrls");
                });

            modelBuilder.Entity("D20Tek.Shurly.Domain.ShortenedUrl.ShortenedUrl", b =>
                {
                    b.OwnsOne("D20Tek.Shurly.Domain.ShortenedUrl.UrlMetadata", "UrlMetadata", b1 =>
                        {
                            b1.Property<Guid>("ShortenedUrlId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ModifiedOn")
                                .HasColumnType("datetime2");

                            b1.Property<Guid>("OwnerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("PublishOn")
                                .HasColumnType("datetime2");

                            b1.Property<int>("State")
                                .HasColumnType("int");

                            b1.Property<string>("Tags")
                                .IsRequired()
                                .HasMaxLength(1024)
                                .HasColumnType("nvarchar(1024)");

                            b1.HasKey("ShortenedUrlId");

                            b1.HasIndex("OwnerId");

                            b1.ToTable("ShortenedUrls");

                            b1.WithOwner()
                                .HasForeignKey("ShortenedUrlId");
                        });

                    b.Navigation("UrlMetadata")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
