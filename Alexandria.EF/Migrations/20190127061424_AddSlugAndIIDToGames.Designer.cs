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
    [Migration("20190127061424_AddSlugAndIIDToGames")]
    partial class AddSlugAndIIDToGames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alexandria.EF.Models.Competition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<Guid>("CompetitionLevelId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid?>("DefaultRoleId");

                    b.Property<string>("Description");

                    b.Property<Guid>("GameId");

                    b.Property<int?>("MaxTeamSize");

                    b.Property<int>("MinTeamSize")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("RulesSlug");

                    b.Property<string>("Slug");

                    b.Property<string>("Title")
                        .HasMaxLength(500);

                    b.Property<string>("TitleCardImageURL");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionLevelId");

                    b.HasIndex("DefaultRoleId");

                    b.HasIndex("GameId");

                    b.HasIndex("Slug");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("Alexandria.EF.Models.CompetitionLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CompetitionLevels");
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

            modelBuilder.Entity("Alexandria.EF.Models.FavoriteCompetition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompetitionId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("FavoriteCompetitions");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("InternalIdentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.Property<string>("Slug");

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

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Alexandria.EF.Models.ProfanityFilter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Severity");

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.ToTable("ProfanityFilters");
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

                    b.Property<string>("Slug");

                    b.Property<int>("TeamState");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("Slug");

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

                    b.Property<bool>("RemoveProtection");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.ToTable("TeamRoles");
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

                    b.Property<string>("Slug");

                    b.Property<DateTimeOffset?>("StartDate");

                    b.Property<int>("State");

                    b.Property<string>("TokenImageURL");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("Slug");

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

                    b.ToTable("TournamentApplications");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Notes");

                    b.Property<int>("State");

                    b.Property<Guid>("TournamentApplicationId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentApplicationId");

                    b.ToTable("TournamentApplicationHistory");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DefaultValue");

                    b.Property<int>("FieldType");

                    b.Property<bool>("Optional");

                    b.Property<string>("QuestionKey");

                    b.Property<string>("SelectOptions");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentApplicationQuestions");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationQuestionAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("TournamentApplicationId");

                    b.Property<Guid>("TournamentApplicationQuestionId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentApplicationId");

                    b.HasIndex("TournamentApplicationQuestionId");

                    b.ToTable("TournamentApplicationQuestionAnswers");
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

            modelBuilder.Entity("Alexandria.EF.Models.TournamentParticipation", b =>
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

                    b.ToTable("TournamentParticipations");
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentParticipationHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Notes");

                    b.Property<int>("State");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentParticipationHistories");
                });

            modelBuilder.Entity("Alexandria.EF.Models.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarURL");

                    b.Property<DateTimeOffset?>("Birthday");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Alexandria.EF.Models.Competition", b =>
                {
                    b.HasOne("Alexandria.EF.Models.CompetitionLevel", "CompetitionLevel")
                        .WithMany("Competitions")
                        .HasForeignKey("CompetitionLevelId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.TeamRole", "DefaultRole")
                        .WithMany()
                        .HasForeignKey("DefaultRoleId")
                        .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity("Alexandria.EF.Models.FavoriteCompetition", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Competition", "Competition")
                        .WithMany()
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("FavoriteCompetitions")
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
                        .WithMany("TeamInvites")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.UserProfile", "UserProfile")
                        .WithMany("TeamInvites")
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
                        .WithMany("TeamRoles")
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
                        .WithMany("TournamentApplications")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentApplications")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.TournamentApplication", "TournamentApplication")
                        .WithMany("TournamentApplicationHistories")
                        .HasForeignKey("TournamentApplicationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationQuestion", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentApplicationQuestions")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentApplicationQuestionAnswer", b =>
                {
                    b.HasOne("Alexandria.EF.Models.TournamentApplication", "TournamentApplication")
                        .WithMany("TournamentApplicationQuestionAnswers")
                        .HasForeignKey("TournamentApplicationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.TournamentApplicationQuestion", "TournamentApplicationQuestion")
                        .WithMany("TournamentApplicationQuestionAnswers")
                        .HasForeignKey("TournamentApplicationQuestionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentHistories")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentParticipation", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany("TournamentParticipations")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentParticipations")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Alexandria.EF.Models.TournamentParticipationHistory", b =>
                {
                    b.HasOne("Alexandria.EF.Models.Team", "Team")
                        .WithMany("TournamentParticipationHistories")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Alexandria.EF.Models.Tournament", "Tournament")
                        .WithMany("TournamentParticipationHistories")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
