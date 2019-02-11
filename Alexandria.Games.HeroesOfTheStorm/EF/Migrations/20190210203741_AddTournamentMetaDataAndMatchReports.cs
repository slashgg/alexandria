using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Migrations
{
    public partial class AddTournamentMetaDataAndMatchReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentSettings",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    ReplayUploadRequired = table.Column<bool>(nullable: false),
                    MapBanCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapBans",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    MatchSeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapBans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapBans_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "heroesofthestorm",
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentMaps",
                schema: "heroesofthestorm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TournamentSettingsId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentMaps_Maps_MapId",
                        column: x => x.MapId,
                        principalSchema: "heroesofthestorm",
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentMaps_TournamentSettings_TournamentSettingsId",
                        column: x => x.TournamentSettingsId,
                        principalSchema: "heroesofthestorm",
                        principalTable: "TournamentSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "heroesofthestorm",
                table: "Maps",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("10b18967-2cf4-48c0-8d75-755cb5bfd412"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(5631), "Alterac Pass" },
                    { new Guid("15abd880-303c-448a-9e3b-880dda1d497c"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6257), "Battlefield of Eternity" },
                    { new Guid("e184140d-af7e-4bef-84e0-7decef025f98"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6262), "Blackheart's Bay" },
                    { new Guid("66e5022b-6a15-4574-89c6-0a1585ad292f"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6264), "Braxis Holdout" },
                    { new Guid("002a1da1-38bc-4982-9b3c-68da83bb817b"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6265), "Cursed Hollow" },
                    { new Guid("de50dbcc-9f0e-4e81-9629-03ef51c934fc"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6282), "Dragon Shire" },
                    { new Guid("46fd0708-e4ae-49c8-9099-f8a30dcbfed6"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6284), "Garden of Terror" },
                    { new Guid("5d59187d-b680-450b-a0e7-4f4687cda113"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6285), "Hanamura Temple" },
                    { new Guid("444b674d-8d9c-4f1a-a638-a7104d97ce0d"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6286), "Haunted Mines" },
                    { new Guid("9d09de78-cbd9-4074-8d8b-6b760f866165"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6289), "Infernal Shrines" },
                    { new Guid("ba7fb66a-fde8-4665-b269-478827e86740"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6290), "Lost Caverns" },
                    { new Guid("9e7f4d53-9cf7-4653-8c30-4895a26491b2"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6291), "Sky Temple" },
                    { new Guid("0d85de37-edb7-4af6-82ba-b9acc6bd2612"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6292), "Tomb of the Spider Queen" },
                    { new Guid("8390ad52-2dd3-4209-942b-75ba50b42baa"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6296), "Towers of Doom" },
                    { new Guid("6125d9b9-7c73-455e-ba66-f480680eb084"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6297), "Volskaya Foundry" },
                    { new Guid("3e065907-bebf-4708-8eac-c020b9e03b1c"), new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6298), "Warhead Junction" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapBans_MapId",
                schema: "heroesofthestorm",
                table: "MapBans",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMaps_MapId",
                schema: "heroesofthestorm",
                table: "TournamentMaps",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMaps_TournamentSettingsId",
                schema: "heroesofthestorm",
                table: "TournamentMaps",
                column: "TournamentSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapBans",
                schema: "heroesofthestorm");

            migrationBuilder.DropTable(
                name: "TournamentMaps",
                schema: "heroesofthestorm");

            migrationBuilder.DropTable(
                name: "Maps",
                schema: "heroesofthestorm");

            migrationBuilder.DropTable(
                name: "TournamentSettings",
                schema: "heroesofthestorm");
        }
    }
}
