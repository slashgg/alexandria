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
    [Migration("20190128040543_AddExternalRanking")]
    partial class AddExternalRanking
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("heroesofthestorm")
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alexandria.EF.Models.HeroesOfTheStorm.ExternalRanking", b =>
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
#pragma warning restore 612, 618
        }
    }
}
