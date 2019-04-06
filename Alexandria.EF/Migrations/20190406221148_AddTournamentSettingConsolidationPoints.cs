using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentSettingConsolidationPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Settings_RoundRobinConsolidationPoint",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_RoundRobinConsolidationPointMinimumWins",
                table: "Tournaments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings_RoundRobinConsolidationPoint",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Settings_RoundRobinConsolidationPointMinimumWins",
                table: "Tournaments");
        }
    }
}
