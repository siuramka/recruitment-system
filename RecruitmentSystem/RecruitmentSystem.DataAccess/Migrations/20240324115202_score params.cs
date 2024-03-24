using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class scoreparams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CorrelationBoostModifer",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CorrelationBoostValue",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "X1X2Average",
                table: "FinalScores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrelationBoostModifer",
                table: "FinalScores");

            migrationBuilder.DropColumn(
                name: "CorrelationBoostValue",
                table: "FinalScores");

            migrationBuilder.DropColumn(
                name: "X1X2Average",
                table: "FinalScores");
        }
    }
}
