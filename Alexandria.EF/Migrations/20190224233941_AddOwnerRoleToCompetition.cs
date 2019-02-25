using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddOwnerRoleToCompetition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerRoleId",
                table: "Competitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OwnerRoleId",
                table: "Competitions",
                column: "OwnerRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_TeamRoles_OwnerRoleId",
                table: "Competitions",
                column: "OwnerRoleId",
                principalTable: "TeamRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_TeamRoles_OwnerRoleId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_OwnerRoleId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "OwnerRoleId",
                table: "Competitions");
        }
    }
}
