using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D20Tek.Shurly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OwnerIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrls_UrlMetadata_OwnerId",
                table: "ShortenedUrls",
                column: "UrlMetadata_OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShortenedUrls_UrlMetadata_OwnerId",
                table: "ShortenedUrls");
        }
    }
}
