﻿// <auto-generated />
using System;
using Alexandria.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alexandria.EF.Migrations
{
    [DbContext(typeof(AlexandriaContext))]
    [Migration("20181223024631_AddRoles")]
    partial class AddRoles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alexandria.EF.Models.Competition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("GameId");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("Alexandria.EF.Models.ExternalAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("ExternalId");

                    b.Property<string>("Name");

                    b.Property<int>("Provider");

                    b.Property<string>("Token");

                    b.Property<Guid>("UserProfileId");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("ExternalAccount");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ARN")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreviation");

                    b.Property<Guid>("CompetitionId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("LogoURL");

                    b.Property<string>("Name");

                    b.Property<int>("TeamState");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamInvite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid?>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("TeamInvites");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamMembership", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("TeamRoleId");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamRoleId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("TeamMemberships");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamMembershipHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Notes");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("TeamMembershipHistory");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompetitionId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Permissions");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.ToTable("TeamRole");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ApplicationRequired");

                    b.Property<bool>("CanSignup");

                    b.Property<Guid>("CompetitionId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTimeOffset?>("EndDate");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<DateTimeOffset?>("SignupCloseDate");

                    b.Property<DateTimeOffset?>("SignupOpenDate");

                    b.Property<DateTimeOffset?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentApplication");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Notes");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid?>("TournamentApplicationId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TournamentApplicationId");

                    b.ToTable("TournamentApplicationHistory");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Notes")
                        .HasMaxLength(500);

                    b.Property<int>("State");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentHistory");
                });

            modelBuilder.Entity("Alexandria.EF.Models.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarURL");

                    b.Property<DateTime?>("Birthday");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName")
                        .IsUnique()
                        .HasFilter("[DisplayName] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Competition", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Game", "Game")
                        .WithMany("Competitions")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.ExternalAccount", b =>
                {
                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("ExternalAccounts")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.Permission", b =>
                {
                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("Permissions")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.Team", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Competition", "Competition")
                        .WithMany("Teams")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamInvite", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamMembership", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany("TeamMemberships")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.TeamRole", "TeamRole")
                        .WithMany("TeamMemberships")
                        .HasForeignKey("TeamRoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("TeamMemberships")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamMembershipHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany("TeamMembershipHistories")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("TeamMembershipHistories")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TeamRole", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Competition", "Competition")
                        .WithMany()
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.Tournament", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Competition", "Competition")
                        .WithMany("Tournaments")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplication", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentApplications")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany("TournamentApplicationHistories")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.TournamentApplication")
                        .WithMany("TournamentApplicationHistories")
                        .HasForeignKey("TournamentApplicationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentHistories")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
