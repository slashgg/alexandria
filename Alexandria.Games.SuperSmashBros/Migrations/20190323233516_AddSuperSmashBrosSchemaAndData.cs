using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Games.SuperSmashBros.Migrations
{
    public partial class AddSuperSmashBrosSchemaAndData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "supersmashbros");

            migrationBuilder.CreateTable(
                name: "Fighters",
                schema: "supersmashbros",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchReports",
                schema: "supersmashbros",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FighterPicks",
                schema: "supersmashbros",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    FighterId = table.Column<Guid>(nullable: false),
                    MatchReportId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FighterPicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FighterPicks_Fighters_FighterId",
                        column: x => x.FighterId,
                        principalSchema: "supersmashbros",
                        principalTable: "Fighters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FighterPicks_MatchReports_MatchReportId",
                        column: x => x.MatchReportId,
                        principalSchema: "supersmashbros",
                        principalTable: "MatchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FighterPicks_FighterId",
                schema: "supersmashbros",
                table: "FighterPicks",
                column: "FighterId");

            migrationBuilder.CreateIndex(
                name: "IX_FighterPicks_MatchReportId",
                schema: "supersmashbros",
                table: "FighterPicks",
                column: "MatchReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FighterPicks",
                schema: "supersmashbros");

            migrationBuilder.DropTable(
                name: "Fighters",
                schema: "supersmashbros");

            migrationBuilder.DropTable(
                name: "MatchReports",
                schema: "supersmashbros");
        }
    }
}
