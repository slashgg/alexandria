using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddScheduleRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchSeriesScheduleRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    ProposedTimeSlot = table.Column<DateTimeOffset>(nullable: false),
                    OriginTeamId = table.Column<Guid>(nullable: false),
                    TargetTeamId = table.Column<Guid>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeriesScheduleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchSeriesScheduleRequests_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchSeriesScheduleRequests_Teams_OriginTeamId",
                        column: x => x.OriginTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchSeriesScheduleRequests_Teams_TargetTeamId",
                        column: x => x.TargetTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesScheduleRequests_MatchSeriesId",
                table: "MatchSeriesScheduleRequests",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesScheduleRequests_OriginTeamId",
                table: "MatchSeriesScheduleRequests",
                column: "OriginTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeriesScheduleRequests_TargetTeamId",
                table: "MatchSeriesScheduleRequests",
                column: "TargetTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchSeriesScheduleRequests");
        }
    }
}
