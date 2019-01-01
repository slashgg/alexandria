using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentIdToTournamentApplicationHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationHistory_Teams_TeamId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropIndex(
                name: "IX_TournamentApplicationHistory_TeamId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "TournamentApplicationHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "TournamentApplicationId",
                table: "TournamentApplicationHistory",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TournamentApplicationId",
                table: "TournamentApplicationHistory",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "TournamentApplicationHistory",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationHistory_TeamId",
                table: "TournamentApplicationHistory",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationHistory_Teams_TeamId",
                table: "TournamentApplicationHistory",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
