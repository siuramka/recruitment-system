using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class REFRESH_TOKEN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "SiteUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "SiteUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Internship",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "SiteUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "SiteUsers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternshipId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Internship_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internship",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_InternshipId",
                table: "Skill",
                column: "InternshipId");
        }
    }
}
