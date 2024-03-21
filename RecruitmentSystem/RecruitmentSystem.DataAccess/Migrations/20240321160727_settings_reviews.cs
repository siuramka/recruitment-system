using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class settings_reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "AiScoreWeight",
                table: "Settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyScoreWeight",
                table: "Settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "InternshipId",
                table: "Settings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TotalScoreWeight",
                table: "Settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_InternshipId",
                table: "Settings",
                column: "InternshipId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Internships_InternshipId",
                table: "Settings",
                column: "InternshipId",
                principalTable: "Internships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Internships_InternshipId",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_InternshipId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AiScoreWeight",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CompanyScoreWeight",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "InternshipId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TotalScoreWeight",
                table: "Settings");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReviewStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationId",
                table: "Reviews",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CompanyId",
                table: "Reviews",
                column: "CompanyId");
        }
    }
}
