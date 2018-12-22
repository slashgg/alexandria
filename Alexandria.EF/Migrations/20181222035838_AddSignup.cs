using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddSignup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApplicationRequired",
                table: "Tournaments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TeamInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamInvites_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamInvites_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembershipHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    NOtes = table.Column<string>(nullable: true),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembershipHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembershipHistory_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembershipHistory_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentApplicationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentApplicationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentApplicationHistory_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_TeamId",
                table: "TeamInvites",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_UserProfileId",
                table: "TeamInvites",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipHistory_TeamId",
                table: "TeamMembershipHistory",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipHistory_UserProfileId",
                table: "TeamMembershipHistory",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationHistory_TeamId",
                table: "TournamentApplicationHistory",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamInvites");

            migrationBuilder.DropTable(
                name: "TeamMembershipHistory");

            migrationBuilder.DropTable(
                name: "TournamentApplicationHistory");

            migrationBuilder.DropColumn(
                name: "ApplicationRequired",
                table: "Tournaments");
        }
    }
}
