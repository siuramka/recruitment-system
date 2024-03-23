using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class forgot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalScore_Applications_ApplicationId",
                table: "FinalScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalScore",
                table: "FinalScore");

            migrationBuilder.RenameTable(
                name: "FinalScore",
                newName: "FinalScores");

            migrationBuilder.RenameIndex(
                name: "IX_FinalScore_ApplicationId",
                table: "FinalScores",
                newName: "IX_FinalScores_ApplicationId");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<double>(
                name: "AiScoreX1",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CompanyScoreX2",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalScores",
                table: "FinalScores",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalScores_Applications_ApplicationId",
                table: "FinalScores",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalScores_Applications_ApplicationId",
                table: "FinalScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalScores",
                table: "FinalScores");

            migrationBuilder.DropColumn(
                name: "AiScoreX1",
                table: "FinalScores");

            migrationBuilder.DropColumn(
                name: "CompanyScoreX2",
                table: "FinalScores");

            migrationBuilder.RenameTable(
                name: "FinalScores",
                newName: "FinalScore");

            migrationBuilder.RenameIndex(
                name: "IX_FinalScores_ApplicationId",
                table: "FinalScore",
                newName: "IX_FinalScore_ApplicationId");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "FinalScore",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalScore",
                table: "FinalScore",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalScore_Applications_ApplicationId",
                table: "FinalScore",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
