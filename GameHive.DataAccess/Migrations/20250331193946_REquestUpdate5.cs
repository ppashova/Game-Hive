using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class REquestUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestImages_GameRequests_GameRequestId",
                table: "RequestImages");

            migrationBuilder.DropIndex(
                name: "IX_RequestImages_GameRequestId",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "GameRequestId",
                table: "RequestImages");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestImages_GameRequests_RequestId",
                table: "RequestImages",
                column: "RequestId",
                principalTable: "GameRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestImages_GameRequests_RequestId",
                table: "RequestImages");

            migrationBuilder.AddColumn<int>(
                name: "GameRequestId",
                table: "RequestImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestImages_GameRequestId",
                table: "RequestImages",
                column: "GameRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestImages_GameRequests_GameRequestId",
                table: "RequestImages",
                column: "GameRequestId",
                principalTable: "GameRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
