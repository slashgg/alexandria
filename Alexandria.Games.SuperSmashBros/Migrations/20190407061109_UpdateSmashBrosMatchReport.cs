using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Games.SuperSmashBros.Migrations
{
    public partial class UpdateSmashBrosMatchReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                schema: "supersmashbros",
                table: "FighterPicks",
                newName: "TeamId");

            migrationBuilder.AddColumn<Guid>(
                name: "MatchId",
                schema: "supersmashbros",
                table: "FighterPicks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchId",
                schema: "supersmashbros",
                table: "FighterPicks");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "supersmashbros",
                table: "FighterPicks",
                newName: "UserProfileId");
        }
    }
}
