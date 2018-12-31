using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddCompetitionDefaultRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultRoleId",
                table: "Competitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_DefaultRoleId",
                table: "Competitions",
                column: "DefaultRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_TeamRoles_DefaultRoleId",
                table: "Competitions",
                column: "DefaultRoleId",
                principalTable: "TeamRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_TeamRoles_DefaultRoleId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_DefaultRoleId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "DefaultRoleId",
                table: "Competitions");
        }
    }
}
