using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Settings_RoundRobinDrawPoints",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_RoundRobinLossPoints",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_RoundRobinWinPoints",
                table: "Tournaments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings_RoundRobinDrawPoints",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Settings_RoundRobinLossPoints",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Settings_RoundRobinWinPoints",
                table: "Tournaments");
        }
    }
}
