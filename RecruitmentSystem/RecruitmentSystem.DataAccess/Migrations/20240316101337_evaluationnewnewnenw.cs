using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class evaluationnewnewnenw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiScore",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "CompanyScore",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "AiScore",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CompanyScore",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewStatus",
                table: "Reviews",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationId",
                table: "Interviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationId",
                table: "Cvs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationId",
                table: "Assessments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AiScore = table.Column<int>(type: "integer", nullable: false),
                    CompanyScore = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_EvaluationId",
                table: "Interviews",
                column: "EvaluationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_EvaluationId",
                table: "Cvs",
                column: "EvaluationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EvaluationId",
                table: "Assessments",
                column: "EvaluationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Evaluations_EvaluationId",
                table: "Assessments",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_Evaluations_EvaluationId",
                table: "Cvs",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Evaluations_EvaluationId",
                table: "Interviews",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Evaluations_EvaluationId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_Evaluations_EvaluationId",
                table: "Cvs");

            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Evaluations_EvaluationId",
                table: "Interviews");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_EvaluationId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_EvaluationId",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_EvaluationId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "Assessments");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewStatus",
                table: "Reviews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AiScore",
                table: "Cvs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyScore",
                table: "Cvs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AiScore",
                table: "Applications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyScore",
                table: "Applications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
