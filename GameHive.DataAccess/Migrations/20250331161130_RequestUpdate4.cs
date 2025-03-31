﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RequestUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests",
                column: "GameId",
                unique: true,
                filter: "[GameId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests",
                column: "GameId",
                unique: true);
        }
    }
}
