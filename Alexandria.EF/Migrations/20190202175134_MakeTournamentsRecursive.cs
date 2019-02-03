using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class MakeTournamentsRecursive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentTournamentId",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_ParentTournamentId",
                table: "Tournaments",
                column: "ParentTournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Tournaments_ParentTournamentId",
                table: "Tournaments",
                column: "ParentTournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Tournaments_ParentTournamentId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_ParentTournamentId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ParentTournamentId",
                table: "Tournaments");
        }
    }
}
