using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RequestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BriefDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    GameIconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameHeaderUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRequests_AspNetUsers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRequests_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestImages",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestImages", x => new { x.RequestId, x.ImageUrl });
                    table.ForeignKey(
                        name: "FK_RequestImages_GameRequests_GameRequestId",
                        column: x => x.GameRequestId,
                        principalTable: "GameRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestTags",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTags", x => new { x.RequestId, x.TagId });
                    table.ForeignKey(
                        name: "FK_RequestTags_GameRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "GameRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_GameId",
                table: "GameRequests",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameRequests_PublisherId",
                table: "GameRequests",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestImages_GameRequestId",
                table: "RequestImages",
                column: "GameRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTags_TagId",
                table: "RequestTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestImages");

            migrationBuilder.DropTable(
                name: "RequestTags");

            migrationBuilder.DropTable(
                name: "GameRequests");

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
