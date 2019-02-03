using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentGroupsAndMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_competitionRankingGroupMemberships_Competitions_CompetitionId",
                table: "competitionRankingGroupMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_competitionRankingGroupMemberships_playerRankingGroups_PlayerRankingGroupId",
                table: "competitionRankingGroupMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_competitionRankingGroupMemberships",
                table: "competitionRankingGroupMemberships");

            migrationBuilder.RenameTable(
                name: "competitionRankingGroupMemberships",
                newName: "CompetitionRankingGroupMemberships");

            migrationBuilder.RenameIndex(
                name: "IX_competitionRankingGroupMemberships_PlayerRankingGroupId",
                table: "CompetitionRankingGroupMemberships",
                newName: "IX_CompetitionRankingGroupMemberships_PlayerRankingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_competitionRankingGroupMemberships_CompetitionId",
                table: "CompetitionRankingGroupMemberships",
                newName: "IX_CompetitionRankingGroupMemberships_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompetitionRankingGroupMemberships",
                table: "CompetitionRankingGroupMemberships",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TournamentGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
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
                    TournamentGroupId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TournamentRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    SeriesGameCount = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false),
                    TournamentGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRounds_TournamentGroups_TournamentGroupId",
                        column: x => x.TournamentGroupId,
                        principalTable: "TournamentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentRounds_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    TournamentRoundId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSeries_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchSeries_TournamentRounds_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "TournamentRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    OutcomeState = table.Column<int>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchParticipants_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchParticipants_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchParticipantResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MatchOutcome = table.Column<int>(nullable: false),
                    MatchParticipantId = table.Column<Guid>(nullable: false),
                    MatchId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchParticipantResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchParticipantResults_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchParticipantResults_MatchParticipants_MatchParticipantId",
                        column: x => x.MatchParticipantId,
                        principalTable: "MatchParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchSeriesId",
                table: "Matches",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipantResults_MatchId",
                table: "MatchParticipantResults",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipantResults_MatchParticipantId",
                table: "MatchParticipantResults",
                column: "MatchParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_MatchSeriesId",
                table: "MatchParticipants",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchParticipants_TeamId",
                table: "MatchParticipants",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeries_GameId",
                table: "MatchSeries",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeries_TournamentRoundId",
                table: "MatchSeries",
                column: "TournamentRoundId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentGroupId",
                table: "TournamentRounds",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentId",
                table: "TournamentRounds",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionRankingGroupMemberships_Competitions_CompetitionId",
                table: "CompetitionRankingGroupMemberships",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionRankingGroupMemberships_playerRankingGroups_PlayerRankingGroupId",
                table: "CompetitionRankingGroupMemberships",
                column: "PlayerRankingGroupId",
                principalTable: "playerRankingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionRankingGroupMemberships_Competitions_CompetitionId",
                table: "CompetitionRankingGroupMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionRankingGroupMemberships_playerRankingGroups_PlayerRankingGroupId",
                table: "CompetitionRankingGroupMemberships");

            migrationBuilder.DropTable(
                name: "MatchParticipantResults");

            migrationBuilder.DropTable(
                name: "TournamentGroupMemberships");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "MatchParticipants");

            migrationBuilder.DropTable(
                name: "MatchSeries");

            migrationBuilder.DropTable(
                name: "TournamentRounds");

            migrationBuilder.DropTable(
                name: "TournamentGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompetitionRankingGroupMemberships",
                table: "CompetitionRankingGroupMemberships");

            migrationBuilder.RenameTable(
                name: "CompetitionRankingGroupMemberships",
                newName: "competitionRankingGroupMemberships");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionRankingGroupMemberships_PlayerRankingGroupId",
                table: "competitionRankingGroupMemberships",
                newName: "IX_competitionRankingGroupMemberships_PlayerRankingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionRankingGroupMemberships_CompetitionId",
                table: "competitionRankingGroupMemberships",
                newName: "IX_competitionRankingGroupMemberships_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_competitionRankingGroupMemberships",
                table: "competitionRankingGroupMemberships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_competitionRankingGroupMemberships_Competitions_CompetitionId",
                table: "competitionRankingGroupMemberships",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_competitionRankingGroupMemberships_playerRankingGroups_PlayerRankingGroupId",
                table: "competitionRankingGroupMemberships",
                column: "PlayerRankingGroupId",
                principalTable: "playerRankingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
