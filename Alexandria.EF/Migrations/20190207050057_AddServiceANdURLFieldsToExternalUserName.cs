using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddServiceANdURLFieldsToExternalUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoURL",
                table: "ExternalUserName",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "ExternalUserName",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoURL",
                table: "ExternalUserName");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "ExternalUserName");
        }
    }
}
