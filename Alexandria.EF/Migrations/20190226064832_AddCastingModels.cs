using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddCastingModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CastingClaimRequired",
                table: "MatchSeries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MatchSeriesCastingClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeriesCastingClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSeriesCastingClaims_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchSeriesCastingClaims_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchSeriesCastings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    StreamingURL = table.Column<string>(nullable: true),
                    VODURL = table.Column<string>(nullable: true),
                    StartsAt = table.Column<DateTimeOffset>(nullable: true),
                    MatchSeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeriesCastings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSeriesCastings_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchSeriesCastingParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false),
                    MatchSeriesCastingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeriesCastingParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSeriesCastingParticipants_MatchSeriesCastings_MatchSeriesCastingId",
                        column: x => x.MatchSeriesCastingId,
                        principalTable: "MatchSeriesCastings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchSeriesCastingParticipants_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesCastingClaims_MatchSeriesId",
                table: "MatchSeriesCastingClaims",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesCastingClaims_UserProfileId",
                table: "MatchSeriesCastingClaims",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesCastingParticipants_MatchSeriesCastingId",
                table: "MatchSeriesCastingParticipants",
                column: "MatchSeriesCastingId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesCastingParticipants_UserProfileId",
                table: "MatchSeriesCastingParticipants",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesCastings_MatchSeriesId",
                table: "MatchSeriesCastings",
                column: "MatchSeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchSeriesCastingClaims");

            migrationBuilder.DropTable(
                name: "MatchSeriesCastingParticipants");

            migrationBuilder.DropTable(
                name: "MatchSeriesCastings");

            migrationBuilder.DropColumn(
                name: "CastingClaimRequired",
                table: "MatchSeries");
        }
    }
}
