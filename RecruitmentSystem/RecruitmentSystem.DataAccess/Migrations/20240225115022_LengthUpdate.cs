using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LengthUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_InternshipStep_InternshipStepStepId_Internship~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Internship_InternshipId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_Internship_InternshipId",
                table: "Cvs");

            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Companys_CompanyId",
                table: "Internship");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipStep_Internship_InternshipId",
                table: "InternshipStep");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipStep_Steps_StepId",
                table: "InternshipStep");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InternshipStep",
                table: "InternshipStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Internship",
                table: "Internship");

            migrationBuilder.RenameTable(
                name: "InternshipStep",
                newName: "InternshipSteps");

            migrationBuilder.RenameTable(
                name: "Internship",
                newName: "Internships");

            migrationBuilder.RenameIndex(
                name: "IX_InternshipStep_InternshipId",
                table: "InternshipSteps",
                newName: "IX_InternshipSteps_InternshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Internship_CompanyId",
                table: "Internships",
                newName: "IX_Internships_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Internships",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Internships",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Internships",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InternshipSteps",
                table: "InternshipSteps",
                columns: new[] { "StepId", "InternshipId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Internships",
                table: "Internships",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    SiteUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_InternshipSteps_InternshipStepStepId_Internshi~",
                table: "Applications",
                columns: new[] { "InternshipStepStepId", "InternshipStepInternshipId" },
                principalTable: "InternshipSteps",
                principalColumns: new[] { "StepId", "InternshipId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Internships_InternshipId",
                table: "Applications",
                column: "InternshipId",
                principalTable: "Internships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_Internships_InternshipId",
                table: "Cvs",
                column: "InternshipId",
                principalTable: "Internships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipSteps_Internships_InternshipId",
                table: "InternshipSteps",
                column: "InternshipId",
                principalTable: "Internships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipSteps_Steps_StepId",
                table: "InternshipSteps",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Internships_Companys_CompanyId",
                table: "Internships",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_InternshipSteps_InternshipStepStepId_Internshi~",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Internships_InternshipId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_Internships_InternshipId",
                table: "Cvs");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipSteps_Internships_InternshipId",
                table: "InternshipSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipSteps_Steps_StepId",
                table: "InternshipSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_Internships_Companys_CompanyId",
                table: "Internships");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Internships",
                table: "Internships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InternshipSteps",
                table: "InternshipSteps");

            migrationBuilder.RenameTable(
                name: "Internships",
                newName: "Internship");

            migrationBuilder.RenameTable(
                name: "InternshipSteps",
                newName: "InternshipStep");

            migrationBuilder.RenameIndex(
                name: "IX_Internships_CompanyId",
                table: "Internship",
                newName: "IX_Internship_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_InternshipSteps_InternshipId",
                table: "InternshipStep",
                newName: "IX_InternshipStep_InternshipId");

            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Internship",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Internship",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Internship",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Internship",
                table: "Internship",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InternshipStep",
                table: "InternshipStep",
                columns: new[] { "StepId", "InternshipId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_InternshipStep_InternshipStepStepId_Internship~",
                table: "Applications",
                columns: new[] { "InternshipStepStepId", "InternshipStepInternshipId" },
                principalTable: "InternshipStep",
                principalColumns: new[] { "StepId", "InternshipId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Internship_InternshipId",
                table: "Applications",
                column: "InternshipId",
                principalTable: "Internship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_Internship_InternshipId",
                table: "Cvs",
                column: "InternshipId",
                principalTable: "Internship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Companys_CompanyId",
                table: "Internship",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipStep_Internship_InternshipId",
                table: "InternshipStep",
                column: "InternshipId",
                principalTable: "Internship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipStep_Steps_StepId",
                table: "InternshipStep",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
