using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InterviewChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternshipId",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "SiteUserId",
                table: "Interviews",
                newName: "ApplicationId");

            migrationBuilder.AddColumn<int>(
                name: "MinutesLength",
                table: "Interviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Interviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_ApplicationId",
                table: "Interviews",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Applications_ApplicationId",
                table: "Interviews",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Applications_ApplicationId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_ApplicationId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "MinutesLength",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "Interviews",
                newName: "SiteUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "InternshipId",
                table: "Interviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
