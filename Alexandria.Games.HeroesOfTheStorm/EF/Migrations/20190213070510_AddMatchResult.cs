using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Migrations
{
    public partial class AddMatchResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchReports",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MatchId = table.Column<Guid>(nullable: false),
                    ReplayURL = table.Column<string>(nullable: true),
                    ReplayParsed = table.Column<bool>(nullable: false),
                    ReplayedParsedAt = table.Column<DateTimeOffset>(nullable: true),
                    MapId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchReports_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "heroesofthestorm",
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchReports_MapId",
                schema: "heroesofthestorm",
                table: "MatchReports",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchReports",
                schema: "heroesofthestorm");
        }
    }
}
