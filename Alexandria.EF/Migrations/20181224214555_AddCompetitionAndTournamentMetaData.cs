using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddCompetitionAndTournamentMetaData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenImageURL",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Competitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Competitions",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleCardImageURL",
                table: "Competitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenImageURL",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "TitleCardImageURL",
                table: "Competitions");
        }
    }
}
