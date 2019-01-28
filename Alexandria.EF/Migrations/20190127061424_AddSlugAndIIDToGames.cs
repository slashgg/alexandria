using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddSlugAndIIDToGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternalIdentifier",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalIdentifier",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Games");
        }
    }
}
