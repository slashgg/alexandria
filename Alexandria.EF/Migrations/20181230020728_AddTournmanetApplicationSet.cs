using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddTournmanetApplicationSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplication_Teams_TeamId",
                table: "TournamentApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplication_Tournaments_TournamentId",
                table: "TournamentApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationQuestionAnswers_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationQuestionAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentApplication",
                table: "TournamentApplication");

            migrationBuilder.RenameTable(
                name: "TournamentApplication",
                newName: "TournamentApplications");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentApplication_TournamentId",
                table: "TournamentApplications",
                newName: "IX_TournamentApplications_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentApplication_TeamId",
                table: "TournamentApplications",
                newName: "IX_TournamentApplications_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentApplications",
                table: "TournamentApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplications_TournamentApplicationId",
                table: "TournamentApplicationHistory",
                column: "TournamentApplicationId",
                principalTable: "TournamentApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationQuestionAnswers_TournamentApplications_TournamentApplicationId",
                table: "TournamentApplicationQuestionAnswers",
                column: "TournamentApplicationId",
                principalTable: "TournamentApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplications_Teams_TeamId",
                table: "TournamentApplications",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplications_Tournaments_TournamentId",
                table: "TournamentApplications",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplications_TournamentApplicationId",
                table: "TournamentApplicationHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplicationQuestionAnswers_TournamentApplications_TournamentApplicationId",
                table: "TournamentApplicationQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplications_Teams_TeamId",
                table: "TournamentApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentApplications_Tournaments_TournamentId",
                table: "TournamentApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentApplications",
                table: "TournamentApplications");

            migrationBuilder.RenameTable(
                name: "TournamentApplications",
                newName: "TournamentApplication");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentApplications_TournamentId",
                table: "TournamentApplication",
                newName: "IX_TournamentApplication_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentApplications_TeamId",
                table: "TournamentApplication",
                newName: "IX_TournamentApplication_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentApplication",
                table: "TournamentApplication",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplication_Teams_TeamId",
                table: "TournamentApplication",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplication_Tournaments_TournamentId",
                table: "TournamentApplication",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationHistory_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationHistory",
                column: "TournamentApplicationId",
                principalTable: "TournamentApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentApplicationQuestionAnswers_TournamentApplication_TournamentApplicationId",
                table: "TournamentApplicationQuestionAnswers",
                column: "TournamentApplicationId",
                principalTable: "TournamentApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
