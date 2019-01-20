using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.EF.Migrations
{
    public partial class AddCompetitionLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var recreationalGuid = Guid.NewGuid();

            migrationBuilder.AddColumn<Guid>(
                name: "CompetitionLevelId",
                table: "Competitions",
                nullable: false,
                defaultValue: recreationalGuid);

            migrationBuilder.CreateTable(
                name: "CompetitionLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionLevels", x => x.Id);
                });

      migrationBuilder.InsertData(
        table: "CompetitionLevels",
        columns: new[] { "Id", "CreatedAt", "Name", "Level" },
        values: new object[] { recreationalGuid, DateTime.UtcNow, "Recreational", "1" }
      );

      migrationBuilder.CreateIndex(
                name: "IX_Competitions_CompetitionLevelId",
                table: "Competitions",
                column: "CompetitionLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_CompetitionLevels_CompetitionLevelId",
                table: "Competitions",
                column: "CompetitionLevelId",
                principalTable: "CompetitionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_CompetitionLevels_CompetitionLevelId",
                table: "Competitions");

            migrationBuilder.DropTable(
                name: "CompetitionLevels");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_CompetitionLevelId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "CompetitionLevelId",
                table: "Competitions");
        }
    }
}
