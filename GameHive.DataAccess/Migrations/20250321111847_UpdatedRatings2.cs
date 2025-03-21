using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRatings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_AspNetUsers_UserId1",
                table: "UserRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_UserId1",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserRatings");

            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "UserRatings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_UserId",
                table: "UserRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_AspNetUsers_UserId",
                table: "UserRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_AspNetUsers_UserId",
                table: "UserRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_UserId",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "UserRatings");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserRatings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRatings",
                table: "UserRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_UserId1",
                table: "UserRatings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_AspNetUsers_UserId1",
                table: "UserRatings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
