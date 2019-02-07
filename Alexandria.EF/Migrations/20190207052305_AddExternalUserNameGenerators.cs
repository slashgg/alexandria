using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddExternalUserNameGenerators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalUserNameGenerators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    LogoURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUserNameGenerators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameUserNameGenerators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    ExternalUserNameGeneratorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUserNameGenerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameUserNameGenerators_ExternalUserNameGenerators_ExternalUserNameGeneratorId",
                        column: x => x.ExternalUserNameGeneratorId,
                        principalTable: "ExternalUserNameGenerators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameUserNameGenerators_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameUserNameGenerators_ExternalUserNameGeneratorId",
                table: "GameUserNameGenerators",
                column: "ExternalUserNameGeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserNameGenerators_GameId",
                table: "GameUserNameGenerators",
                column: "GameId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUserNameGenerators");

            migrationBuilder.DropTable(
                name: "ExternalUserNameGenerators");
        }
    }
}
