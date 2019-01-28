using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Migrations
{
    public partial class AddExternalRanking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "heroesofthestorm");

            migrationBuilder.CreateTable(
                name: "ExternalRankings",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Ranking = table.Column<int>(nullable: true),
                    GameMode = table.Column<string>(nullable: true),
                    MMRSource = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    BattleNetRegion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalRankings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalRankings",
                schema: "heroesofthestorm");
        }
    }
}
