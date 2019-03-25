using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddMinMaxSeriesGameCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeriesMaxGameCount",
                table: "TournamentRounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeriesMinGameCount",
                table: "TournamentRounds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeriesMaxGameCount",
                table: "TournamentRounds");

            migrationBuilder.DropColumn(
                name: "SeriesMinGameCount",
                table: "TournamentRounds");
        }
    }
}
