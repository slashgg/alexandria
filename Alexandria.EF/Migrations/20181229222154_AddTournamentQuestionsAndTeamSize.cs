using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournamentQuestionsAndTeamSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxTeamSize",
                table: "Competitions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinTeamSize",
                table: "Competitions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "TournamentApplicationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    QuestionKey = table.Column<string>(nullable: true),
                    FieldType = table.Column<int>(nullable: false),
                    SelectOptions = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    DefaultValue = table.Column<string>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentApplicationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentApplicationQuestions_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentApplicationQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    TournamentApplicationId = table.Column<Guid>(nullable: false),
                    TournamentApplicationQuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentApplicationQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentApplicationQuestionAnswers_TournamentApplication_TournamentApplicationId",
                        column: x => x.TournamentApplicationId,
                        principalTable: "TournamentApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentApplicationQuestionAnswers_TournamentApplicationQuestions_TournamentApplicationQuestionId",
                        column: x => x.TournamentApplicationQuestionId,
                        principalTable: "TournamentApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationQuestionAnswers_TournamentApplicationId",
                table: "TournamentApplicationQuestionAnswers",
                column: "TournamentApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationQuestionAnswers_TournamentApplicationQuestionId",
                table: "TournamentApplicationQuestionAnswers",
                column: "TournamentApplicationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentApplicationQuestions_TournamentId",
                table: "TournamentApplicationQuestions",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentApplicationQuestionAnswers");

            migrationBuilder.DropTable(
                name: "TournamentApplicationQuestions");

            migrationBuilder.DropColumn(
                name: "MaxTeamSize",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "MinTeamSize",
                table: "Competitions");
        }
    }
}
