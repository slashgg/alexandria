﻿// <auto-generated />
using System;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Migrations
{
    [DbContext(typeof(HeroesOfTheStormContext))]
    [Migration("20190210203741_AddTournamentMetaDataAndMatchReports")]
    partial class AddTournamentMetaDataAndMatchReports
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("heroesofthestorm")
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.ExternalRanking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BattleNetRegion");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("GameMode");

                    b.Property<int>("MMRSource");

                    b.Property<int?>("Ranking");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.ToTable("ExternalRankings");
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.Map", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Maps");

                    b.HasData(
                        new
                        {
                            Id = new Guid("10b18967-2cf4-48c0-8d75-755cb5bfd412"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(5631),
                            Name = "Alterac Pass"
                        },
                        new
                        {
                            Id = new Guid("15abd880-303c-448a-9e3b-880dda1d497c"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6257),
                            Name = "Battlefield of Eternity"
                        },
                        new
                        {
                            Id = new Guid("e184140d-af7e-4bef-84e0-7decef025f98"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6262),
                            Name = "Blackheart's Bay"
                        },
                        new
                        {
                            Id = new Guid("66e5022b-6a15-4574-89c6-0a1585ad292f"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6264),
                            Name = "Braxis Holdout"
                        },
                        new
                        {
                            Id = new Guid("002a1da1-38bc-4982-9b3c-68da83bb817b"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6265),
                            Name = "Cursed Hollow"
                        },
                        new
                        {
                            Id = new Guid("de50dbcc-9f0e-4e81-9629-03ef51c934fc"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6282),
                            Name = "Dragon Shire"
                        },
                        new
                        {
                            Id = new Guid("46fd0708-e4ae-49c8-9099-f8a30dcbfed6"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6284),
                            Name = "Garden of Terror"
                        },
                        new
                        {
                            Id = new Guid("5d59187d-b680-450b-a0e7-4f4687cda113"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6285),
                            Name = "Hanamura Temple"
                        },
                        new
                        {
                            Id = new Guid("444b674d-8d9c-4f1a-a638-a7104d97ce0d"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6286),
                            Name = "Haunted Mines"
                        },
                        new
                        {
                            Id = new Guid("9d09de78-cbd9-4074-8d8b-6b760f866165"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6289),
                            Name = "Infernal Shrines"
                        },
                        new
                        {
                            Id = new Guid("ba7fb66a-fde8-4665-b269-478827e86740"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6290),
                            Name = "Lost Caverns"
                        },
                        new
                        {
                            Id = new Guid("9e7f4d53-9cf7-4653-8c30-4895a26491b2"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6291),
                            Name = "Sky Temple"
                        },
                        new
                        {
                            Id = new Guid("0d85de37-edb7-4af6-82ba-b9acc6bd2612"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6292),
                            Name = "Tomb of the Spider Queen"
                        },
                        new
                        {
                            Id = new Guid("8390ad52-2dd3-4209-942b-75ba50b42baa"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6296),
                            Name = "Towers of Doom"
                        },
                        new
                        {
                            Id = new Guid("6125d9b9-7c73-455e-ba66-f480680eb084"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6297),
                            Name = "Volskaya Foundry"
                        },
                        new
                        {
                            Id = new Guid("3e065907-bebf-4708-8eac-c020b9e03b1c"),
                            CreatedAt = new DateTime(2019, 2, 10, 20, 37, 40, 955, DateTimeKind.Utc).AddTicks(6298),
                            Name = "Warhead Junction"
                        });
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.MapBan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("MapId");

                    b.Property<Guid>("MatchSeriesId");

                    b.Property<Guid>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.ToTable("MapBans");
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.TournamentMap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("MapId");

                    b.Property<Guid>("TournamentSettingsId");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.HasIndex("TournamentSettingsId");

                    b.ToTable("TournamentMaps");
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.TournamentSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("MapBanCount");

                    b.Property<bool>("ReplayUploadRequired");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.ToTable("TournamentSettings");
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.MapBan", b =>
                {
                    b.HasOne("Alexandria.Games.HeroesOfTheStorm.EF.Models.Map", "Map")
                        .WithMany("MapBans")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.Games.HeroesOfTheStorm.EF.Models.TournamentMap", b =>
                {
                    b.HasOne("Alexandria.Games.HeroesOfTheStorm.EF.Models.Map", "Map")
                        .WithMany("TournamentMaps")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.Games.HeroesOfTheStorm.EF.Models.TournamentSettings", "TournamentSettings")
                        .WithMany("TournamentMaps")
                        .HasForeignKey("TournamentSettingsId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
