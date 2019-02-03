using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class RemoveTournamentGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentRounds_TournamentGroups_TournamentGroupId",
                table: "TournamentRounds");

            migrationBuilder.DropTable(
                name: "TournamentGroupMemberships");

            migrationBuilder.DropTable(
                name: "TournamentGroups");

            migrationBuilder.DropIndex(
                name: "IX_TournamentRounds_TournamentGroupId",
                table: "TournamentRounds");

            migrationBuilder.DropColumn(
                name: "TournamentGroupId",
                table: "TournamentRounds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentGroupId",
                table: "TournamentRounds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentGroups_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentGroupMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    TournamentGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentGroupMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentGroupMemberships_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentGroupMemberships_TournamentGroups_TournamentGroupId",
                        column: x => x.TournamentGroupId,
                        principalTable: "TournamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentGroupId",
                table: "TournamentRounds",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroupMemberships_TeamId",
                table: "TournamentGroupMemberships",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroupMemberships_TournamentGroupId",
                table: "TournamentGroupMemberships",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroups_TournamentId",
                table: "TournamentGroups",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentRounds_TournamentGroups_TournamentGroupId",
                table: "TournamentRounds",
                column: "TournamentGroupId",
                principalTable: "TournamentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
