using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BugFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageTime",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "BadReviews",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "NeutralReviews",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "PositiveReviews",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "ScoreStatus",
                table: "Applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AverageTime",
                table: "Companys",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BadReviews",
                table: "Companys",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NeutralReviews",
                table: "Companys",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositiveReviews",
                table: "Companys",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ScoreStatus",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
