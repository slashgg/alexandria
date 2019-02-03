using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentTypeAndSeriesPerRound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeriesPerRound",
                table: "TournamentRounds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "SeriesPerRound",
                table: "TournamentRounds");
        }
    }
}
