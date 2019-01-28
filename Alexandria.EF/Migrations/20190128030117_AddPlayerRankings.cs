using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddPlayerRankings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "playerRankingGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playerRankingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_playerRankingGroups_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "competitionRankingGroupMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CompetitionId = table.Column<Guid>(nullable: false),
                    PlayerRankingGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competitionRankingGroupMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competitionRankingGroupMemberships_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_competitionRankingGroupMemberships_playerRankingGroups_PlayerRankingGroupId",
                        column: x => x.PlayerRankingGroupId,
                        principalTable: "playerRankingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRankings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MMR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    PlayerRankingGroupId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerRankings_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerRankings_playerRankingGroups_PlayerRankingGroupId",
                        column: x => x.PlayerRankingGroupId,
                        principalTable: "playerRankingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerRankings_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_competitionRankingGroupMemberships_CompetitionId",
                table: "competitionRankingGroupMemberships",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_competitionRankingGroupMemberships_PlayerRankingGroupId",
                table: "competitionRankingGroupMemberships",
                column: "PlayerRankingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_playerRankingGroups_GameId",
                table: "playerRankingGroups",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRankings_GameId",
                table: "PlayerRankings",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRankings_PlayerRankingGroupId",
                table: "PlayerRankings",
                column: "PlayerRankingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRankings_UserProfileId",
                table: "PlayerRankings",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "competitionRankingGroupMemberships");

            migrationBuilder.DropTable(
                name: "PlayerRankings");

            migrationBuilder.DropTable(
                name: "playerRankingGroups");
        }
    }
}
