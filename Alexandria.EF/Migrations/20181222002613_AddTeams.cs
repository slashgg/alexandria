using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CompetitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamRole_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    LogoURL = table.Column<string>(nullable: true),
                    TeamState = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    TeamRoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberships_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMemberships_TeamRole_TeamRoleId",
                        column: x => x.TeamRoleId,
                        principalTable: "TeamRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMemberships_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberships_TeamId",
                table: "TeamMemberships",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberships_TeamRoleId",
                table: "TeamMemberships",
                column: "TeamRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberships_UserProfileId",
                table: "TeamMemberships",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRole_CompetitionId",
                table: "TeamRole",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CompetitionId",
                table: "Teams",
                column: "CompetitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMemberships");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "TeamRole");
        }
    }
}
