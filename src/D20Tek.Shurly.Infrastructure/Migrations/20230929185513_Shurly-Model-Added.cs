using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D20Tek.Shurly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShurlyModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortenedUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LongUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ShortUrlCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    UrlMetadata_State = table.Column<int>(type: "int", nullable: false),
                    UrlMetadata_OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlMetadata_CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlMetadata_PublishOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlMetadata_ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_ShortUrlCode",
                table: "ShortenedUrls",
                column: "ShortUrlCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortenedUrls");
        }
    }
}
