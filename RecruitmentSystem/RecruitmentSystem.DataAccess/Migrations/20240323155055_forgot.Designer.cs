﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RecruitmentSystem.DataAccess;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    [DbContext(typeof(RecruitmentDbContext))]
    [Migration("20240323155055_forgot")]
    partial class forgot
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InternshipId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InternshipStepId")
                        .HasColumnType("uuid");

                    b.Property<string>("ScoreStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteUserId")
                        .HasColumnType("text");

                    b.Property<string>("Skills")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("InternshipId");

                    b.HasIndex("InternshipStepId");

                    b.HasIndex("SiteUserId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Assessment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EvaluationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.HasIndex("EvaluationId")
                        .IsUnique();

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AverageTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BadReviews")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NeutralReviews")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PositiveReviews")
                        .HasColumnType("integer");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Companys");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Cv", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EvaluationId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("FileContent")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("InternshipId")
                        .HasColumnType("uuid");

                    b.Property<string>("SiteUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("EvaluationId")
                        .IsUnique();

                    b.HasIndex("InternshipId");

                    b.HasIndex("SiteUserId");

                    b.ToTable("Cvs");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Decision", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AiCandidateSummary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AiStagesReview")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AiStagesScore")
                        .HasColumnType("integer");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<int>("CompanyStagesScores")
                        .HasColumnType("integer");

                    b.Property<string>("CompanySummary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Decisions");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Evaluation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AiScore")
                        .HasColumnType("integer");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<int>("CompanyScore")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.FinalScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("AiScoreX1")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<double>("CompanyScoreX2")
                        .HasColumnType("double precision");

                    b.Property<double>("Correlation")
                        .HasColumnType("double precision");

                    b.Property<string>("Review")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.ToTable("FinalScores");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Internship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRemote")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Requirements")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Skills")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SlotsAvailable")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TakenSlots")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Internships");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.InternshipStep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("InternshipId")
                        .HasColumnType("uuid");

                    b.Property<int>("PositionAscending")
                        .HasColumnType("integer");

                    b.Property<Guid>("StepId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InternshipId");

                    b.HasIndex("StepId");

                    b.ToTable("InternshipSteps");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Interview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EvaluationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MinutesLength")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("EvaluationId")
                        .IsUnique();

                    b.ToTable("Interviews");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AiScoreWeight")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyScoreWeight")
                        .HasColumnType("integer");

                    b.Property<Guid>("InternshipId")
                        .HasColumnType("uuid");

                    b.Property<int>("TotalScoreWeight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InternshipId")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.SiteUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Step", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StepType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Application", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Internship", "Internship")
                        .WithMany()
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.InternshipStep", "InternshipStep")
                        .WithMany("Applications")
                        .HasForeignKey("InternshipStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", "SiteUser")
                        .WithMany()
                        .HasForeignKey("SiteUserId");

                    b.Navigation("Internship");

                    b.Navigation("InternshipStep");

                    b.Navigation("SiteUser");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Assessment", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithOne("Assessment")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.Assessment", "ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.Evaluation", null)
                        .WithOne("Assessment")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.Assessment", "EvaluationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Cv", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.Evaluation", null)
                        .WithOne("Cv")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.Cv", "EvaluationId");

                    b.HasOne("RecruitmentSystem.Domain.Models.Internship", "Internship")
                        .WithMany()
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.SiteUser", "SiteUser")
                        .WithMany()
                        .HasForeignKey("SiteUserId");

                    b.Navigation("Application");

                    b.Navigation("Internship");

                    b.Navigation("SiteUser");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Decision", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Evaluation", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithMany("Evaluations")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.FinalScore", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithOne("FinalScore")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.FinalScore", "ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Internship", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Company", "Company")
                        .WithMany("Internships")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.InternshipStep", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Internship", "Internship")
                        .WithMany("InternshipSteps")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.Step", "Step")
                        .WithMany("InternshipSteps")
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Internship");

                    b.Navigation("Step");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Interview", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecruitmentSystem.Domain.Models.Evaluation", null)
                        .WithOne("Interview")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.Interview", "EvaluationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Setting", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Internship", "Internship")
                        .WithOne("Setting")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.Setting", "InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Internship");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.SiteUser", b =>
                {
                    b.HasOne("RecruitmentSystem.Domain.Models.Company", "Company")
                        .WithOne("SiteUser")
                        .HasForeignKey("RecruitmentSystem.Domain.Models.SiteUser", "CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Application", b =>
                {
                    b.Navigation("Assessment");

                    b.Navigation("Evaluations");

                    b.Navigation("FinalScore");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Company", b =>
                {
                    b.Navigation("Internships");

                    b.Navigation("SiteUser")
                        .IsRequired();
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Evaluation", b =>
                {
                    b.Navigation("Assessment");

                    b.Navigation("Cv");

                    b.Navigation("Interview");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Internship", b =>
                {
                    b.Navigation("InternshipSteps");

                    b.Navigation("Setting");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.InternshipStep", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("RecruitmentSystem.Domain.Models.Step", b =>
                {
                    b.Navigation("InternshipSteps");
                });
#pragma warning restore 612, 618
        }
    }
}
