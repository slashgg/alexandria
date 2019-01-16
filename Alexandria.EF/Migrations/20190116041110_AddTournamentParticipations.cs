using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentParticipations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TournamentParticipationHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentParticipationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentParticipationHistories_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentParticipationHistories_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentParticipations",
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
                    table.PrimaryKey("PK_TournamentParticipations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentParticipations_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentParticipations_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipationHistories_TeamId",
                table: "TournamentParticipationHistories",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipationHistories_TournamentId",
                table: "TournamentParticipationHistories",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipations_TeamId",
                table: "TournamentParticipations",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipations_TournamentId",
                table: "TournamentParticipations",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentParticipationHistories");

            migrationBuilder.DropTable(
                name: "TournamentParticipations");
        }
    }
}
