using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class evaluations_applicaiton_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "Evaluations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ApplicationId",
                table: "Evaluations",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Applications_ApplicationId",
                table: "Evaluations",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Applications_ApplicationId",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_ApplicationId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Evaluations");
        }
    }
}
