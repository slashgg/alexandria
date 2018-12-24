using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NOtes",
                table: "TeamMembershipHistory",
                newName: "Notes");

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentApplicationId",
                table: "TournamentApplicationHistory",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentApplication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentApplication_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentApplication_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationHistory_TournamentApplicationId",
                table: "TournamentApplicationHistory",
                column: "TournamentApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplication_TeamId",
                table: "TournamentApplication",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplication_TournamentId",
                table: "TournamentApplication",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationHistory",
                column: "TournamentApplicationId",
                principalTable: "TournamentApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropTable(
                name: "TournamentApplication");

            migrationBuilder.DropIndex(
                name: "IX_TournamentApplicationHistory_TournamentApplicationId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropColumn(
                name: "TournamentApplicationId",
                table: "TournamentApplicationHistory");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "TeamMembershipHistory",
                newName: "NOtes");
        }
    }
}
