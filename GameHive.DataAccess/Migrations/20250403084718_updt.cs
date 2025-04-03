using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests");

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests");

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests",
                column: "GameId",
                unique: true,
                filter: "[GameId] IS NOT NULL");
        }
    }
}
