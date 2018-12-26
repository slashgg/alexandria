using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class Catchup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMemberships_TeamRole_TeamRoleId",
                table: "TeamMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamRole_Competitions_CompetitionId",
                table: "TeamRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamRole",
                table: "TeamRole");

            migrationBuilder.RenameTable(
                name: "TeamRole",
                newName: "TeamRoles");

            migrationBuilder.RenameIndex(
                name: "IX_TeamRole_CompetitionId",
                table: "TeamRoles",
                newName: "IX_TeamRoles_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamRoles",
                table: "TeamRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMemberships_TeamRoles_TeamRoleId",
                table: "TeamMemberships",
                column: "TeamRoleId",
                principalTable: "TeamRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamRoles_Competitions_CompetitionId",
                table: "TeamRoles",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMemberships_TeamRoles_TeamRoleId",
                table: "TeamMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamRoles_Competitions_CompetitionId",
                table: "TeamRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamRoles",
                table: "TeamRoles");

            migrationBuilder.RenameTable(
                name: "TeamRoles",
                newName: "TeamRole");

            migrationBuilder.RenameIndex(
                name: "IX_TeamRoles_CompetitionId",
                table: "TeamRole",
                newName: "IX_TeamRole_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamRole",
                table: "TeamRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMemberships_TeamRole_TeamRoleId",
                table: "TeamMemberships",
                column: "TeamRoleId",
                principalTable: "TeamRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamRole_Competitions_CompetitionId",
                table: "TeamRole",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
